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
    public class ListClientsQueryHandler : IRequestHandler<ListClientsQuery, OperationResult<PagedResult<Client>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListClientsQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<Client>>> Handle(ListClientsQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult<PagedResult<Client>>();

            if (request.Page <= 0 || request.PageSize <= 0)
            {
                return result;
            }

            var query = _dbContext.Clients.AsQueryable();

            if (!string.IsNullOrEmpty(request.PersonalCode))
            {
                query = query.Where(c => c.PersonalCode.Contains(request.PersonalCode));
            }

            result.Value = await query
                .OrderBy(c => c.ClientId)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
