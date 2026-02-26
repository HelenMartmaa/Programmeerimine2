using System.Collections.Generic;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.AppointmentDocuments
{
    public class GetAppointmentDocumentQuery : IRequest<OperationResult<AppointmentDocumentDetailsDto>>
    {
        public int Id { get; set; }
    }
}