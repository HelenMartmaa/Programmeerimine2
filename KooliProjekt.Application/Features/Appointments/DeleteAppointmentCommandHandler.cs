using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Appointments
{
    public class DeleteAppointmentCommandHandler : IRequestHandler<DeleteAppointmentCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteAppointmentCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteAppointmentCommand request, CancellationToken cancellationToken)
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

            var appointment = await _dbContext.Appointments
                .Include(a => a.Documents)
                .Include(a => a.Invoice)
                    .ThenInclude(i => i.InvoiceRows)
                .FirstOrDefaultAsync(a => a.AppointmentId == request.Id);

            if (appointment == null)
            {
                return result;
            }

            // Delete AppointmentDocuments
            if (appointment.Documents != null)
            {
                _dbContext.AppointmentDocuments.RemoveRange(appointment.Documents);
            }

            // Delete InvoiceRows and Invoice
            if (appointment.Invoice != null)
            {
                if (appointment.Invoice.InvoiceRows != null)
                {
                    _dbContext.InvoiceRows.RemoveRange(appointment.Invoice.InvoiceRows);
                }
                _dbContext.Invoices.Remove(appointment.Invoice);
            }

            // Delete Appointment
            _dbContext.Appointments.Remove(appointment);
            await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}