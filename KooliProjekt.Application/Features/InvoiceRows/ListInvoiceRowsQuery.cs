using System.Collections.Generic;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.InvoiceRows
{
    public class ListInvoiceRowsQuery : IRequest<OperationResult<IList<KooliProjekt.Application.Data.InvoiceRow>>>
    {
    }
}
