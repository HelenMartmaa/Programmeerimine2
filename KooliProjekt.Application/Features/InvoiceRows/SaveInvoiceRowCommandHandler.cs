using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.InvoiceRows
{
    public class SaveInvoiceRowCommandHandler : IRequestHandler<SaveInvoiceRowCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveInvoiceRowCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null) throw new ArgumentNullException(nameof(dbContext));
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveInvoiceRowCommand request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var result = new OperationResult();
            if (request.InvoiceRowId < 0)
            {
                return result.AddPropertyError(nameof(request.InvoiceRowId), "Id cannot be negative.");
            }

            var invoiceRow = new InvoiceRow();
            if (request.InvoiceRowId == 0)
            {
                await _dbContext.InvoiceRows.AddAsync(invoiceRow);
            }
            else
            {
                invoiceRow = await _dbContext.InvoiceRows.FindAsync(request.InvoiceRowId);
                if (invoiceRow == null)
                {
                    return result.AddPropertyError(nameof(request.InvoiceRowId), "InvoiceRow with the specified Id does not exist.");
                }
            }

            invoiceRow.InvoiceId = request.InvoiceId;
            invoiceRow.ServiceDescription = request.ServiceDescription;
            invoiceRow.Fee = request.Fee;
            invoiceRow.Quantity = request.Quantity;
            invoiceRow.Discount = request.Discount;

            await _dbContext.SaveChangesAsync();
            return result;
        }
    }
}