using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Clients
{
    public class DeleteClientCommandHandler : IRequestHandler<DeleteClientCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteClientCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteClientCommand request, CancellationToken cancellationToken)
        {
            // All client appointments
            var appointmentIds = await _dbContext.Appointments
                .Where(a => a.ClientId == request.Id)
                .Select(a => a.AppointmentId)
                .ToListAsync(cancellationToken);

            // Delete all related AppointmentDocuments
            await _dbContext.AppointmentDocuments
                .Where(ad => appointmentIds.Contains(ad.AppointmentId))
                .ExecuteDeleteAsync(cancellationToken);

            // Delete all InvoiceRows through Invoices
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
                .Where(a => a.ClientId == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            // Delete Client
            await _dbContext.Clients
                .Where(c => c.ClientId == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            return new OperationResult();
        }
    }
}