using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.DoctorUnavailabilities
{
    public class GetDoctorUnavailabilityQueryHandler : IRequestHandler<GetDoctorUnavailabilityQuery, OperationResult<object>>
    {
        private readonly IDoctorUnavailabilityRepository _doctorUnavailabilityRepository;

        public GetDoctorUnavailabilityQueryHandler(IDoctorUnavailabilityRepository doctorUnavailabilityRepository)
        {
            _doctorUnavailabilityRepository = doctorUnavailabilityRepository;
        }

        public async Task<OperationResult<object>> Handle(GetDoctorUnavailabilityQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();
            var unavailability = await _doctorUnavailabilityRepository.GetByIdAsync(request.Id);

            result.Value = new
            {
                Id = unavailability.UnavailabilityId,
                DoctorId = unavailability.DoctorId,
                StartDate = unavailability.StartDate,
                EndDate = unavailability.EndDate,
                Reason = unavailability.Reason,
                Doctor = unavailability.Doctor != null ? new
                {
                    Id = unavailability.Doctor.DoctorId,
                    Name = $"{unavailability.Doctor.User.FirstName} {unavailability.Doctor.User.LastName}",
                    Specialization = unavailability.Doctor.Specialization
                } : null
            };

            return result;
        }
    }
}