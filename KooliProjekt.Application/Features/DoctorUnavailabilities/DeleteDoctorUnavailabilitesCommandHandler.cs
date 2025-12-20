using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.DoctorUnavailabilities
{
    public class DeleteDoctorUnavailabilitiesCommandHandler : IRequestHandler<DeleteDoctorUnavailabilitiesCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteDoctorUnavailabilitiesCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteDoctorUnavailabilitiesCommand request, CancellationToken cancellationToken)
        {
            await _dbContext.DoctorUnavailabilities
                .Where(du => du.UnavailabilityId == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            return new OperationResult();
        }
    }
}