using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Invoices
{
    public class SaveInvoiceCommandHandler : IRequestHandler<SaveInvoiceCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveInvoiceCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null) throw new ArgumentNullException(nameof(dbContext));
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveInvoiceCommand request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var result = new OperationResult();
            if (request.InvoiceId < 0)
            {
                return result.AddPropertyError(nameof(request.InvoiceId), "Id cannot be negative.");
            }

            var invoice = new Invoice();
            if (request.InvoiceId == 0)
            {
                await _dbContext.Invoices.AddAsync(invoice);
            }
            else
            {
                invoice = await _dbContext.Invoices.FindAsync(request.InvoiceId);
                if (invoice == null)
                {
                    return result.AddPropertyError(nameof(request.InvoiceId), "Invoice with the specified Id does not exist.");
                }
            }

            invoice.AppointmentId = request.AppointmentId;
            invoice.InvoiceDate = request.InvoiceDate;
            invoice.DueDate = request.DueDate;
            invoice.TotalBeforeVat = request.TotalBeforeVat;
            invoice.TotalWithVat = request.TotalWithVat;
            invoice.IsPaid = request.IsPaid;
            invoice.PaidAt = request.PaidAt;
            invoice.InvoiceNum = request.InvoiceNum;

            await _dbContext.SaveChangesAsync();
            return result;
        }
    }
}