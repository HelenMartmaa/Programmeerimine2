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
    public class ListAdministratorsQueryHandler : IRequestHandler<ListAdministratorsQuery, OperationResult<PagedResult<KooliProjekt.Application.Data.Administrator>>>
    {
        private readonly ApplicationDbContext _dbContext;
        public ListAdministratorsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<KooliProjekt.Application.Data.Administrator>>> Handle(ListAdministratorsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<KooliProjekt.Application.Data.Administrator>>();
            result.Value = await _dbContext
                .Administrators
                .OrderBy(list => list.AdminId)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
