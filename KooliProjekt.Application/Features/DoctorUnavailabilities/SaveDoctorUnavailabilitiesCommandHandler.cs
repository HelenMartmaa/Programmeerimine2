using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.DoctorUnavailabilities
{
    public class SaveDoctorUnavailabilityCommandHandler : IRequestHandler<SaveDoctorUnavailabilityCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveDoctorUnavailabilityCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null) throw new ArgumentNullException(nameof(dbContext));
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveDoctorUnavailabilityCommand request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var result = new OperationResult();
            if (request.UnavailabilityId < 0)
            {
                return result.AddPropertyError(nameof(request.UnavailabilityId), "Id cannot be negative.");
            }

            var unavailability = new DoctorUnavailability();
            if (request.UnavailabilityId == 0)
            {
                await _dbContext.DoctorUnavailabilities.AddAsync(unavailability);
            }
            else
            {
                unavailability = await _dbContext.DoctorUnavailabilities.FindAsync(request.UnavailabilityId);
                if (unavailability == null)
                {
                    return result.AddPropertyError(nameof(request.UnavailabilityId), "DoctorUnavailability with the specified Id does not exist.");
                }
            }

            unavailability.DoctorId = request.DoctorId;
            unavailability.StartDate = request.StartDate;
            unavailability.EndDate = request.EndDate;
            unavailability.Reason = request.Reason;

            await _dbContext.SaveChangesAsync();
            return result;
        }
    }
}