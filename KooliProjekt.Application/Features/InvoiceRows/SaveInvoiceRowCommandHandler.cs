using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.InvoiceRows
{
    public class SaveInvoiceRowCommandHandler : IRequestHandler<SaveInvoiceRowCommand, OperationResult>
    {
        private readonly IInvoiceRowRepository _invoiceRowRepository;

        public SaveInvoiceRowCommandHandler(IInvoiceRowRepository invoiceRowRepository)
        {
            _invoiceRowRepository = invoiceRowRepository;
        }

        public async Task<OperationResult> Handle(SaveInvoiceRowCommand request, CancellationToken cancellationToken)
        {
            var invoiceRow = new InvoiceRow();
            
            if (request.InvoiceRowId != 0)
            {
                invoiceRow = await _invoiceRowRepository.GetByIdAsync(request.InvoiceRowId);
            }

            invoiceRow.InvoiceId = request.InvoiceId;
            invoiceRow.ServiceDescription = request.ServiceDescription;
            invoiceRow.Fee = request.Fee;
            invoiceRow.Quantity = request.Quantity;
            invoiceRow.Discount = request.Discount;

            await _invoiceRowRepository.SaveAsync(invoiceRow);

            return new OperationResult();
        }
    }
}