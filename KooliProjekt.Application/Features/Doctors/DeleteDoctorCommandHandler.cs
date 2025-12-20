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
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteDoctorCommand request, CancellationToken cancellationToken)
        {
            // All doctor appointments
            var appointmentIds = await _dbContext.Appointments
                .Where(a => a.DoctorId == request.Id)
                .Select(a => a.AppointmentId)
                .ToListAsync(cancellationToken);

            // Delete AppointmentDocuments
            await _dbContext.AppointmentDocuments
                .Where(ad => appointmentIds.Contains(ad.AppointmentId))
                .ExecuteDeleteAsync(cancellationToken);

            // Delete InvoiceRows
            var invoiceIds = await _dbContext.Invoices
                .Where(i => appointmentIds.Contains(i.AppointmentId))
                .Select(i => i.InvoiceId)
                .ToListAsync(cancellationToken);

            await _dbContext.InvoiceRows
                .Where(ir => invoiceIds.Contains(ir.InvoiceId))
                .ExecuteDeleteAsync(cancellationToken);

            // Delete Invoices
            await _dbContext.Invoices
                .Where(i => appointmentIds.Contains(i.AppointmentId))
                .ExecuteDeleteAsync(cancellationToken);

            // Delete Appointments
            await _dbContext.Appointments
                .Where(a => a.DoctorId == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            // Delete DoctorUnavailabilities
            await _dbContext.DoctorUnavailabilities
                .Where(du => du.DoctorId == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            // Delete Doctor
            await _dbContext.Doctors
                .Where(d => d.DoctorId == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            return new OperationResult();
        }
    }
}