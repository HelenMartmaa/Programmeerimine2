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

namespace KooliProjekt.Application.Features.User
{
    public class ListUserQueryHandler : IRequestHandler<ListUserQuery, OperationResult<IList<KooliProjekt.Application.Data.User>>>
    {
        private readonly ApplicationDbContext _dbContext;
        public ListUserQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<IList<KooliProjekt.Application.Data.User>>> Handle(ListUserQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<IList<KooliProjekt.Application.Data.User>>();
            result.Value = await _dbContext
                .Users
                .OrderBy(u => u.LastName)
                .ToListAsync(cancellationToken); //Allows the caller to cancel the ongoing query/operation

            return result;
        }
    }
}
