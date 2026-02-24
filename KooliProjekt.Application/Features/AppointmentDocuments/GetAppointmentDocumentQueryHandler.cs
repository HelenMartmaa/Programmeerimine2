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
    public class GetAppointmentDocumentQueryHandler : IRequestHandler<GetAppointmentDocumentQuery, OperationResult<object>>
    {
        private readonly IAppointmentDocumentRepository _appointmentDocumentRepository;

        public GetAppointmentDocumentQueryHandler(IAppointmentDocumentRepository appointmentDocumentRepository)
        {
            _appointmentDocumentRepository = appointmentDocumentRepository;
        }

        public async Task<OperationResult<object>> Handle(GetAppointmentDocumentQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();
            var document = await _appointmentDocumentRepository.GetByIdAsync(request.Id);

            result.Value = new
            {
                Id = document.DocumentId,
                AppointmentId = document.AppointmentId,
                DocumentType = document.DocumentType,
                FileName = document.FileName,
                FilePath = document.FilePath,
                UploadedAt = document.UploadedAt,
                FileSize = document.FileSize
            };

            return result;
        }
    }
}