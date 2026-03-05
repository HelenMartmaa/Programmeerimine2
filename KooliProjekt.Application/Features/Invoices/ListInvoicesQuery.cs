using System;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Invoices
{
    [ExcludeFromCodeCoverage]
    public class ListInvoicesQuery : IRequest<OperationResult<PagedResult<Invoice>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }

        public bool? IsPaid { get; set; }
        public string InvoiceNum { get; set; }
    }
}
