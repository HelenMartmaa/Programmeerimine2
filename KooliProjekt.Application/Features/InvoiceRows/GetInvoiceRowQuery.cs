using System.Collections.Generic;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.InvoiceRows
{
    public class GetInvoiceRowQuery : IRequest<OperationResult<InvoiceRowDetailsDto>>
    {
        public int Id { get; set; }
    }
}