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

namespace KooliProjekt.Application.Features.DoctorUnavailability
{
    public class ListDoctorUnavailabilityQueryHandler : IRequestHandler<ListDoctorUnavailabilityQuery, OperationResult<IList<KooliProjekt.Application.Data.DoctorUnavailability>>>
    {
        private readonly ApplicationDbContext _dbContext;
        public ListDoctorUnavailabilityQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<IList<KooliProjekt.Application.Data.DoctorUnavailability>>> Handle(ListDoctorUnavailabilityQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<IList<KooliProjekt.Application.Data.DoctorUnavailability>>();
            result.Value = await _dbContext
                .DoctorUnavailabilities
                .OrderBy(r => r.DoctorId)
                .ToListAsync(cancellationToken); //Allows the caller to cancel the ongoing query/operation

            return result;
        }
    }
}
