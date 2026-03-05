using System;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Administrators
{
    [ExcludeFromCodeCoverage]
    public class ListAdministratorsQuery : IRequest<OperationResult<PagedResult<Administrator>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }

        public string Department { get; set; }
    }
}
