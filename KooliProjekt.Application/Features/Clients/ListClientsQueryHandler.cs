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
    public class ListClientsQueryHandler : IRequestHandler<ListClientsQuery, OperationResult<IList<KooliProjekt.Application.Data.Client>>>
    {
        private readonly ApplicationDbContext _dbContext;
        public ListClientsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<IList<KooliProjekt.Application.Data.Client>>> Handle(ListClientsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<IList<KooliProjekt.Application.Data.Client>>();
            result.Value = await _dbContext
                .Clients
                .OrderBy(r => r.ClientId)
                .ToListAsync(cancellationToken); //Allows the caller to cancel the ongoing query/operation

            return result;
        }
    }
}
