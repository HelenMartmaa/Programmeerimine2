using System;
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
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteClientCommand request, CancellationToken cancellationToken)
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

            var client = await _dbContext.Clients
                .Include(c => c.Appointments)
                    .ThenInclude(a => a.Documents)
                .Include(c => c.Appointments)
                    .ThenInclude(a => a.Invoice)
                        .ThenInclude(i => i.InvoiceRows)
                .FirstOrDefaultAsync(c => c.ClientId == request.Id);

            if (client == null)
            {
                return result;
            }

            // Delete all related data
            foreach (var appointment in client.Appointments)
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

            _dbContext.Appointments.RemoveRange(client.Appointments);
            _dbContext.Clients.Remove(client);
            await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}