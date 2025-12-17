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

namespace KooliProjekt.Application.Features.Users
{
    public class ListUsersQueryHandler : IRequestHandler<ListUsersQuery, OperationResult<PagedResult<KooliProjekt.Application.Data.User>>>
    {
        private readonly ApplicationDbContext _dbContext;
        public ListUsersQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<KooliProjekt.Application.Data.User>>> Handle(ListUsersQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<KooliProjekt.Application.Data.User>>();
            result.Value = await _dbContext
                .Users
                .OrderBy(list => list.LastName)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
