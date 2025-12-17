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
    public class ListInvoicesQueryHandler : IRequestHandler<ListInvoicesQuery, OperationResult<PagedResult<KooliProjekt.Application.Data.Invoice>>>
    {
        private readonly ApplicationDbContext _dbContext;
        public ListInvoicesQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<KooliProjekt.Application.Data.Invoice>>> Handle(ListInvoicesQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<KooliProjekt.Application.Data.Invoice>>();
            result.Value = await _dbContext
                .Invoices
                .OrderBy(list => list.InvoiceId)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
