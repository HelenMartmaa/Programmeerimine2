using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Doctors
{
    public class DeleteDoctorCommandHandler : IRequestHandler<DeleteDoctorCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteDoctorCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteDoctorCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult();

            if (request.Id <= 0)
            {
                return result;
            }

            var doctor = await _dbContext.Doctors
                .Include(d => d.Appointments)
                    .ThenInclude(a => a.Documents)
                .Include(d => d.Appointments)
                    .ThenInclude(a => a.Invoice)
                        .ThenInclude(i => i.InvoiceRows)
                .Include(d => d.Unavailabilities)
                .FirstOrDefaultAsync(d => d.DoctorId == request.Id);

            if (doctor == null)
            {
                return result;
            }

            // Delete all related appointment data
            foreach (var appointment in doctor.Appointments)
            {
                if (appointment.Documents != null)
                {
                    _dbContext.AppointmentDocuments.RemoveRange(appointment.Documents);
                }
                if (appointment.Invoice != null)
                {
                    if (appointment.Invoice.InvoiceRows != null)
                    {
                        _dbContext.InvoiceRows.RemoveRange(appointment.Invoice.InvoiceRows);
                    }
                    _dbContext.Invoices.Remove(appointment.Invoice);
                }
            }

            _dbContext.Appointments.RemoveRange(doctor.Appointments);

            // Delete DoctorUnavailabilities
            if (doctor.Unavailabilities != null)
            {
                _dbContext.DoctorUnavailabilities.RemoveRange(doctor.Unavailabilities);
            }

            _dbContext.Doctors.Remove(doctor);
            await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}