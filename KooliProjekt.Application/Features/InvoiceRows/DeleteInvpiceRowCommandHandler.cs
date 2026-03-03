using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.InvoiceRows
{
    public class DeleteInvoiceRowCommandHandler : IRequestHandler<DeleteInvoiceRowCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteInvoiceRowCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteInvoiceRowCommand request, CancellationToken cancellationToken)
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

            var invoiceRow = await _dbContext.InvoiceRows
                .FirstOrDefaultAsync(r => r.InvoiceRowId == request.Id);

            if (invoiceRow == null)
            {
                return result;
            }

            _dbContext.InvoiceRows.Remove(invoiceRow);
            await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}