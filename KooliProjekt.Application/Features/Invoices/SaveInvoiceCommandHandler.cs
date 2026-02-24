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
    public class SaveInvoiceCommandHandler : IRequestHandler<SaveInvoiceCommand, OperationResult>
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public SaveInvoiceCommandHandler(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<OperationResult> Handle(SaveInvoiceCommand request, CancellationToken cancellationToken)
        {
            var invoice = new Invoice();
            
            if (request.InvoiceId != 0)
            {
                invoice = await _invoiceRepository.GetByIdAsync(request.InvoiceId);
            }

            invoice.AppointmentId = request.AppointmentId;
            invoice.DueDate = request.DueDate;
            invoice.TotalBeforeVat = request.TotalBeforeVat;
            invoice.TotalWithVat = request.TotalWithVat;
            invoice.IsPaid = request.IsPaid;
            invoice.PaidAt = request.PaidAt;
            invoice.InvoiceNum = request.InvoiceNum;
            
            if (request.InvoiceId == 0)
            {
                invoice.InvoiceDate = request.InvoiceDate == default ? System.DateTime.Now : request.InvoiceDate;
            }

            await _invoiceRepository.SaveAsync(invoice);

            return new OperationResult();
        }
    }
}