using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.AppointmentDocuments
{
    public class DeleteAppointmentDocumentsCommandHandler : IRequestHandler<DeleteAppointmentDocumentsCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteAppointmentDocumentsCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteAppointmentDocumentsCommand request, CancellationToken cancellationToken)
        {
            await _dbContext.AppointmentDocuments
                .Where(ad => ad.DocumentId == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            return new OperationResult();
        }
    }
}