using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.AppointmentDocuments
{
    public class GetAppointmentDocumentQueryHandler : IRequestHandler<GetAppointmentDocumentQuery, OperationResult<AppointmentDocumentDetailsDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetAppointmentDocumentQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }
            _dbContext = dbContext;
        }

        public async Task<OperationResult<AppointmentDocumentDetailsDto>> Handle(GetAppointmentDocumentQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult<AppointmentDocumentDetailsDto>();

            if (request.Id <= 0)
            {
                return result;
            }

            result.Value = await _dbContext.AppointmentDocuments
                .Where(d => d.DocumentId == request.Id)
                .Select(d => new AppointmentDocumentDetailsDto
                {
                    Id = d.DocumentId,
                    AppointmentId = d.AppointmentId,
                    DocumentType = d.DocumentType,
                    FileName = d.FileName,
                    FilePath = d.FilePath,
                    UploadedAt = d.UploadedAt,
                    FileSize = d.FileSize
                })
                .FirstOrDefaultAsync();

            return result;
        }
    }
}