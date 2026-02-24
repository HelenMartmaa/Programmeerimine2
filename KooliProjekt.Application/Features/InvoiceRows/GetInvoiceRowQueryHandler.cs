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
    public class GetInvoiceRowQueryHandler : IRequestHandler<GetInvoiceRowQuery, OperationResult<object>>
    {
        private readonly IInvoiceRowRepository _invoiceRowRepository;

        public GetInvoiceRowQueryHandler(IInvoiceRowRepository invoiceRowRepository)
        {
            _invoiceRowRepository = invoiceRowRepository;
        }

        public async Task<OperationResult<object>> Handle(GetInvoiceRowQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();
            var invoiceRow = await _invoiceRowRepository.GetByIdAsync(request.Id);

            result.Value = new
            {
                Id = invoiceRow.InvoiceRowId,
                InvoiceId = invoiceRow.InvoiceId,
                ServiceDescription = invoiceRow.ServiceDescription,
                Fee = invoiceRow.Fee,
                Quantity = invoiceRow.Quantity,
                Discount = invoiceRow.Discount
            };

            return result;
        }
    }
}