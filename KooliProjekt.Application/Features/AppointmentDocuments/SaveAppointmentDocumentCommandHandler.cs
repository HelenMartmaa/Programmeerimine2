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

namespace KooliProjekt.Application.Features.AppointmentDocuments
{
    public class SaveAppointmentDocumentCommandHandler : IRequestHandler<SaveAppointmentDocumentCommand, OperationResult>
    {
        private readonly IAppointmentDocumentRepository _appointmentDocumentRepository;

        public SaveAppointmentDocumentCommandHandler(IAppointmentDocumentRepository appointmentDocumentRepository)
        {
            _appointmentDocumentRepository = appointmentDocumentRepository;
        }

        public async Task<OperationResult> Handle(SaveAppointmentDocumentCommand request, CancellationToken cancellationToken)
        {
            var document = new AppointmentDocument();
            
            if (request.DocumentId != 0)
            {
                document = await _appointmentDocumentRepository.GetByIdAsync(request.DocumentId);
            }

            document.AppointmentId = request.AppointmentId;
            document.DocumentType = request.DocumentType;
            document.FileName = request.FileName;
            document.FilePath = request.FilePath;
            document.FileSize = request.FileSize;
            
            if (request.DocumentId == 0)
            {
                document.UploadedAt = request.UploadedAt == default ? System.DateTime.Now : request.UploadedAt;
            }

            await _appointmentDocumentRepository.SaveAsync(document);

            return new OperationResult();
        }
    }
}