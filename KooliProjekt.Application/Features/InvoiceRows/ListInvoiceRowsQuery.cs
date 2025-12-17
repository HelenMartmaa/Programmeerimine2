using System.Collections.Generic;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.InvoiceRows
{
    public class ListInvoiceRowsQuery : IRequest<OperationResult<PagedResult<KooliProjekt.Application.Data.InvoiceRow>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
