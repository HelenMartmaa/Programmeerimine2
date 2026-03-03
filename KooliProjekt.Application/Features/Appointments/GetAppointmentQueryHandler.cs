using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Appointments
{
    public class GetAppointmentQueryHandler : IRequestHandler<GetAppointmentQuery, OperationResult<AppointmentDetailsDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetAppointmentQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }
            _dbContext = dbContext;
        }

        public async Task<OperationResult<AppointmentDetailsDto>> Handle(GetAppointmentQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult<AppointmentDetailsDto>();

            if (request.Id <= 0)
            {
                return result;
            }

            result.Value = await _dbContext.Appointments
                .Include(a => a.Documents)
                .Where(a => a.AppointmentId == request.Id)
                .Select(a => new AppointmentDetailsDto
                {
                    Id = a.AppointmentId,
                    ClientId = a.ClientId,
                    DoctorId = a.DoctorId,
                    AppointmentDateTime = a.AppointmentDateTime,
                    DurationMinutes = a.DurationMinutes,
                    Status = a.Status,
                    Notes = a.Notes,
                    Documents = a.Documents.Select(d => new AppointmentDocumentDto
                    {
                        Id = d.DocumentId,
                        DocumentType = d.DocumentType,
                        FileName = d.FileName,
                        FileSize = d.FileSize
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            return result;
        }
    }
}