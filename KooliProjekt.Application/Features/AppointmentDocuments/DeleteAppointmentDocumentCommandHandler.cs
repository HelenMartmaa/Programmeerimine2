using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.AppointmentDocuments
{
    public class DeleteAppointmentDocumentCommandHandler : IRequestHandler<DeleteAppointmentDocumentCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteAppointmentDocumentCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteAppointmentDocumentCommand request, CancellationToken cancellationToken)
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

            var document = await _dbContext.AppointmentDocuments
                .FirstOrDefaultAsync(d => d.DocumentId == request.Id);

            if (document == null)
            {
                return result;
            }

            _dbContext.AppointmentDocuments.Remove(document);
            await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}