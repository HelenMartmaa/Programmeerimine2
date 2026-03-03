using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Invoices
{
    public class GetInvoiceQueryHandler : IRequestHandler<GetInvoiceQuery, OperationResult<InvoiceDetailsDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetInvoiceQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }
            _dbContext = dbContext;
        }

        public async Task<OperationResult<InvoiceDetailsDto>> Handle(GetInvoiceQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult<InvoiceDetailsDto>();

            if (request.Id <= 0)
            {
                return result;
            }

            result.Value = await _dbContext.Invoices
                .Include(i => i.InvoiceRows)
                .Where(i => i.InvoiceId == request.Id)
                .Select(i => new InvoiceDetailsDto
                {
                    Id = i.InvoiceId,
                    AppointmentId = i.AppointmentId,
                    InvoiceDate = i.InvoiceDate,
                    DueDate = i.DueDate,
                    TotalBeforeVat = i.TotalBeforeVat,
                    TotalWithVat = i.TotalWithVat,
                    IsPaid = i.IsPaid,
                    InvoiceNum = i.InvoiceNum,
                    InvoiceRows = i.InvoiceRows.Select(r => new InvoiceRowDto
                    {
                        Id = r.InvoiceRowId,
                        ServiceDescription = r.ServiceDescription,
                        Fee = r.Fee,
                        Quantity = r.Quantity,
                        Discount = r.Discount
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            return result;
        }
    }
}