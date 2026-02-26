using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.DoctorUnavailabilities
{
    public class GetDoctorUnavailabilityQueryHandler : IRequestHandler<GetDoctorUnavailabilityQuery, OperationResult<DoctorUnavailabilityDetailsDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetDoctorUnavailabilityQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }
            _dbContext = dbContext;
        }

        public async Task<OperationResult<DoctorUnavailabilityDetailsDto>> Handle(GetDoctorUnavailabilityQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<DoctorUnavailabilityDetailsDto>();

            if (request == null)
            {
                return result;
            }

            if (request.Id <= 0)
            {
                return result;
            }

            result.Value = await _dbContext.DoctorUnavailabilities
                .Where(u => u.UnavailabilityId == request.Id)
                .Select(u => new DoctorUnavailabilityDetailsDto
                {
                    Id = u.UnavailabilityId,
                    DoctorId = u.DoctorId,
                    StartDate = u.StartDate,
                    EndDate = u.EndDate,
                    Reason = u.Reason
                })
                .FirstOrDefaultAsync();

            return result;
        }
    }
}