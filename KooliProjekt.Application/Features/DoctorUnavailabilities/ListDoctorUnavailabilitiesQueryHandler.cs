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
    public class ListDoctorUnavailabilitiesQueryHandler : IRequestHandler<ListDoctorUnavailabilitiesQuery, OperationResult<PagedResult<DoctorUnavailability>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListDoctorUnavailabilitiesQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<DoctorUnavailability>>> Handle(ListDoctorUnavailabilitiesQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult<PagedResult<DoctorUnavailability>>();

            if (request.Page <= 0 || request.PageSize <= 0)
            {
                return result;
            }

            result.Value = await _dbContext
                .DoctorUnavailabilities
                .OrderBy(u => u.DoctorId)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
