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
    public class SaveDoctorUnavailabilityCommandHandler : IRequestHandler<SaveDoctorUnavailabilityCommand, OperationResult>
    {
        private readonly IDoctorUnavailabilityRepository _doctorUnavailabilityRepository;

        public SaveDoctorUnavailabilityCommandHandler(IDoctorUnavailabilityRepository doctorUnavailabilityRepository)
        {
            _doctorUnavailabilityRepository = doctorUnavailabilityRepository;
        }

        public async Task<OperationResult> Handle(SaveDoctorUnavailabilityCommand request, CancellationToken cancellationToken)
        {
            var unavailability = new DoctorUnavailability();
            
            if (request.UnavailabilityId != 0)
            {
                unavailability = await _doctorUnavailabilityRepository.GetByIdAsync(request.UnavailabilityId);
            }

            unavailability.DoctorId = request.DoctorId;
            unavailability.StartDate = request.StartDate;
            unavailability.EndDate = request.EndDate;
            unavailability.Reason = request.Reason;

            await _doctorUnavailabilityRepository.SaveAsync(unavailability);

            return new OperationResult();
        }
    }
}