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
    public class ListUsersQueryHandler : IRequestHandler<ListUsersQuery, OperationResult<PagedResult<User>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListUsersQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<User>>> Handle(ListUsersQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult<PagedResult<User>>();

            if (request.Page <= 0 || request.PageSize <= 0)
            {
                return result;
            }

            var query = _dbContext.Users.AsQueryable();

            if (!string.IsNullOrEmpty(request.FirstName))
            {
                query = query.Where(u => u.FirstName.Contains(request.FirstName));
            }

            if (!string.IsNullOrEmpty(request.LastName))
            {
                query = query.Where(u => u.LastName.Contains(request.LastName));
            }

            if (request.Role.HasValue)
            {
                query = query.Where(u => u.Role == request.Role.Value);
            }

            result.Value = await query
                .OrderBy(u => u.LastName)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
