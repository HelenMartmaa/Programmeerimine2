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

namespace KooliProjekt.Application.Features.InvoiceRow
{
    public class ListInvoiceRowQueryHandler : IRequestHandler<ListInvoiceRowQuery, OperationResult<IList<KooliProjekt.Application.Data.InvoiceRow>>>
    {
        private readonly ApplicationDbContext _dbContext;
        public ListInvoiceRowQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<IList<KooliProjekt.Application.Data.InvoiceRow>>> Handle(ListInvoiceRowQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<IList<KooliProjekt.Application.Data.InvoiceRow>>();
            result.Value = await _dbContext
                .InvoiceRows
                .OrderBy(r => r.InvoiceRowId)
                .ToListAsync(cancellationToken); //Allows the caller to cancel the ongoing query/operation

            return result;
        }
    }
}
