using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.InvoiceRows
{
    public class ListInvoiceRowsQueryHandler : IRequestHandler<ListInvoiceRowsQuery, OperationResult<PagedResult<InvoiceRow>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListInvoiceRowsQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<InvoiceRow>>> Handle(ListInvoiceRowsQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult<PagedResult<InvoiceRow>>();

            if (request.Page <= 0 || request.PageSize <= 0)
            {
                return result;
            }

            var query = _dbContext.InvoiceRows.AsQueryable();

            if (request.InvoiceId.HasValue)
            {
                query = query.Where(r => r.InvoiceId == request.InvoiceId.Value);
            }

            if (!string.IsNullOrEmpty(request.ServiceDescription))
            {
                query = query.Where(r => r.ServiceDescription.Contains(request.ServiceDescription));
            }

            result.Value = await query
                .OrderBy(r => r.InvoiceRowId)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}