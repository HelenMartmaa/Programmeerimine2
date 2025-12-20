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
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteAppointmentCommand request, CancellationToken cancellationToken)
        {
            // Delete AppointmentDocuments
            await _dbContext.AppointmentDocuments
                .Where(ad => ad.AppointmentId == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            // Delete InvoiceRows (through Invoice)
            var invoiceIds = await _dbContext.Invoices
                .Where(i => i.AppointmentId == request.Id)
                .Select(i => i.InvoiceId)
                .ToListAsync(cancellationToken);

            await _dbContext.InvoiceRows
                .Where(ir => invoiceIds.Contains(ir.InvoiceId))
                .ExecuteDeleteAsync(cancellationToken);

            // Delete Invoices
            await _dbContext.Invoices
                .Where(i => i.AppointmentId == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            // Delete Appointment
            await _dbContext.Appointments
                .Where(a => a.AppointmentId == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            return new OperationResult();
        }
    }
}