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

namespace KooliProjekt.Application.Features.Clients
{
    public class ListClientsQueryHandler : IRequestHandler<ListClientsQuery, OperationResult<PagedResult<KooliProjekt.Application.Data.Client>>>
    {
        private readonly ApplicationDbContext _dbContext;
        public ListClientsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<KooliProjekt.Application.Data.Client>>> Handle(ListClientsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<KooliProjekt.Application.Data.Client>>();
            result.Value = await _dbContext
                .Clients
                .OrderBy(list => list.ClientId)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
