using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.InvoiceRows
{
    public class GetInvoiceRowQueryHandler : IRequestHandler<GetInvoiceRowQuery, OperationResult<InvoiceRowDetailsDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetInvoiceRowQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }
            _dbContext = dbContext;
        }

        public async Task<OperationResult<InvoiceRowDetailsDto>> Handle(GetInvoiceRowQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<InvoiceRowDetailsDto>();

            if (request == null)
            {
                return result;
            }

            if (request.Id <= 0)
            {
                return result;
            }

            result.Value = await _dbContext.InvoiceRows
                .Where(r => r.InvoiceRowId == request.Id)
                .Select(r => new InvoiceRowDetailsDto
                {
                    Id = r.InvoiceRowId,
                    InvoiceId = r.InvoiceId,
                    ServiceDescription = r.ServiceDescription,
                    Fee = r.Fee,
                    Quantity = r.Quantity,
                    Discount = r.Discount
                })
                .FirstOrDefaultAsync();

            return result;
        }
    }
}