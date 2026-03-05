using System;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.InvoiceRows
{
    [ExcludeFromCodeCoverage]
    public class ListInvoiceRowsQuery : IRequest<OperationResult<PagedResult<InvoiceRow>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }

        public int? InvoiceId { get; set; }
        public string ServiceDescription { get; set; }
    }
}
