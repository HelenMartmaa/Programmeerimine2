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

namespace KooliProjekt.Application.Features.Invoices
{
    public class ListInvoicesQueryHandler : IRequestHandler<ListInvoicesQuery, OperationResult<PagedResult<Invoice>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListInvoicesQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<Invoice>>> Handle(ListInvoicesQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult<PagedResult<Invoice>>();

            if (request.Page <= 0 || request.PageSize <= 0)
            {
                return result;
            }

            var query = _dbContext.Invoices.AsQueryable();

            if (request.IsPaid.HasValue)
            {
                query = query.Where(i => i.IsPaid == request.IsPaid.Value);
            }

            if (!string.IsNullOrEmpty(request.InvoiceNum))
            {
                query = query.Where(i => i.InvoiceNum.Contains(request.InvoiceNum));
            }

            result.Value = await query
                .OrderBy(i => i.InvoiceId)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}