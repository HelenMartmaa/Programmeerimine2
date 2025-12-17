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

namespace KooliProjekt.Application.Features.DoctorUnavailabilities
{
    public class ListDoctorUnavailabilitiesQueryHandler : IRequestHandler<ListDoctorUnavailabilitiesQuery, OperationResult<PagedResult<KooliProjekt.Application.Data.DoctorUnavailability>>>
    {
        private readonly ApplicationDbContext _dbContext;
        public ListDoctorUnavailabilitiesQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<KooliProjekt.Application.Data.DoctorUnavailability>>> Handle(ListDoctorUnavailabilitiesQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<KooliProjekt.Application.Data.DoctorUnavailability>>();
            result.Value = await _dbContext
                .DoctorUnavailabilities
                .OrderBy(list => list.DoctorId)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
