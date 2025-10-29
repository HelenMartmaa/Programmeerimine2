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
    public class ListAdministratorsQueryHandler : IRequestHandler<ListAdministratorsQuery, OperationResult<IList<KooliProjekt.Application.Data.Administrator>>>
    {
        private readonly ApplicationDbContext _dbContext;
        public ListAdministratorsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<IList<KooliProjekt.Application.Data.Administrator>>> Handle(ListAdministratorsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<IList<KooliProjekt.Application.Data.Administrator>>();
            result.Value = await _dbContext
                .Administrators
                .OrderBy(r => r.AdminId)
                .ToListAsync(cancellationToken); //Allows the caller to cancel the ongoing query/operation

            return result;
        }
    }
}
