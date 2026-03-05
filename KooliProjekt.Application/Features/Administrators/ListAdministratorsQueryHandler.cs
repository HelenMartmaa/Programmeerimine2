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

namespace KooliProjekt.Application.Features.Administrators
{
    public class ListAdministratorsQueryHandler : IRequestHandler<ListAdministratorsQuery, OperationResult<PagedResult<Administrator>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListAdministratorsQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<Administrator>>> Handle(ListAdministratorsQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult<PagedResult<Administrator>>();

            if (request.Page <= 0 || request.PageSize <= 0)
            {
                return result;
            }

            var query = _dbContext.Administrators.AsQueryable();

            if (!string.IsNullOrEmpty(request.Department))
            {
                query = query.Where(a => a.Department.Contains(request.Department));
            }

            result.Value = await query
                .OrderBy(a => a.AdminId)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
