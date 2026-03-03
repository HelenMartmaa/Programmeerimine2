using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.DoctorUnavailabilities
{
    public class DeleteDoctorUnavailabilityCommandHandler : IRequestHandler<DeleteDoctorUnavailabilityCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteDoctorUnavailabilityCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteDoctorUnavailabilityCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult();

            if (request.Id <= 0)
            {
                return result;
            }

            var unavailability = await _dbContext.DoctorUnavailabilities
                .FirstOrDefaultAsync(u => u.UnavailabilityId == request.Id);

            if (unavailability == null)
            {
                return result;
            }

            _dbContext.DoctorUnavailabilities.Remove(unavailability);
            await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}