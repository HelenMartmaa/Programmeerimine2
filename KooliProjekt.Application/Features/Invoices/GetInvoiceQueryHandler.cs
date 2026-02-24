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

namespace KooliProjekt.Application.Features.Invoices
{
    public class GetInvoiceQueryHandler : IRequestHandler<GetInvoiceQuery, OperationResult<object>>
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public GetInvoiceQueryHandler(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<OperationResult<object>> Handle(GetInvoiceQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();
            var invoice = await _invoiceRepository.GetByIdAsync(request.Id);

            result.Value = new
            {
                Id = invoice.InvoiceId,
                AppointmentId = invoice.AppointmentId,
                InvoiceDate = invoice.InvoiceDate,
                DueDate = invoice.DueDate,
                TotalBeforeVat = invoice.TotalBeforeVat,
                TotalWithVat = invoice.TotalWithVat,
                IsPaid = invoice.IsPaid,
                PaidAt = invoice.PaidAt,
                InvoiceNum = invoice.InvoiceNum,
                InvoiceRows = invoice.InvoiceRows?.Select(row => new
                {
                    Id = row.InvoiceRowId,
                    ServiceDescription = row.ServiceDescription,
                    Fee = row.Fee,
                    Quantity = row.Quantity,
                    Discount = row.Discount
                })
            };

            return result;
        }
    }
}