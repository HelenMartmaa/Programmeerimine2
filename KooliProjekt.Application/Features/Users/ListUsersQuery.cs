using System;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Users
{
    [ExcludeFromCodeCoverage]
    public class ListUsersQuery : IRequest<OperationResult<PagedResult<User>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserRole? Role { get; set; }
    }
}
