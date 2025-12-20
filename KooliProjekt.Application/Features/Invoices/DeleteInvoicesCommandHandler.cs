using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Invoices
{
    public class DeleteInvoicesCommandHandler : IRequestHandler<DeleteInvoicesCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteInvoicesCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteInvoicesCommand request, CancellationToken cancellationToken)
        {
            // Delete InvoiceRows
            await _dbContext.InvoiceRows
                .Where(ir => ir.InvoiceId == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            // Delete Invoice
            await _dbContext.Invoices
                .Where(i => i.InvoiceId == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            return new OperationResult();
        }
    }
}