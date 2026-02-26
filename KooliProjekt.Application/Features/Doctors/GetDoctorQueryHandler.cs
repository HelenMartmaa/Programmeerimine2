using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Doctors
{
    public class GetDoctorQueryHandler : IRequestHandler<GetDoctorQuery, OperationResult<DoctorDetailsDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetDoctorQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }
            _dbContext = dbContext;
        }

        public async Task<OperationResult<DoctorDetailsDto>> Handle(GetDoctorQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<DoctorDetailsDto>();

            if (request == null)
            {
                return result;
            }

            if (request.Id <= 0)
            {
                return result;
            }

            result.Value = await _dbContext.Doctors
                .Where(d => d.DoctorId == request.Id)
                .Select(d => new DoctorDetailsDto
                {
                    Id = d.DoctorId,
                    UserId = d.UserId,
                    Specialization = d.Specialization,
                    DocLicenseNum = d.DocLicenseNum,
                    WorkingHoursStart = d.WorkingHoursStart,
                    WorkingHoursEnd = d.WorkingHoursEnd
                })
                .FirstOrDefaultAsync();

            return result;
        }
    }
}