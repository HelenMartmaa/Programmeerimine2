using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.AppointmentDocuments
{
    public class SaveAppointmentDocumentCommandHandler : IRequestHandler<SaveAppointmentDocumentCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveAppointmentDocumentCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null) throw new ArgumentNullException(nameof(dbContext));
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveAppointmentDocumentCommand request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var result = new OperationResult();
            if (request.DocumentId < 0)
            {
                return result.AddPropertyError(nameof(request.DocumentId), "Id cannot be negative.");
            }

            var document = new AppointmentDocument();
            if (request.DocumentId == 0)
            {
                await _dbContext.AppointmentDocuments.AddAsync(document);
            }
            else
            {
                document = await _dbContext.AppointmentDocuments.FindAsync(request.DocumentId);
                if (document == null)
                {
                    return result.AddPropertyError(nameof(request.DocumentId), "AppointmentDocument with the specified Id does not exist.");
                }
            }

            document.AppointmentId = request.AppointmentId;
            document.DocumentType = request.DocumentType;
            document.FileName = request.FileName;
            document.FilePath = request.FilePath;
            document.UploadedAt = request.UploadedAt;
            document.FileSize = request.FileSize;

            await _dbContext.SaveChangesAsync();
            return result;
        }
    }
}