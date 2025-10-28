using System.Collections.Generic;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.AppointmentDocument
{
    public class ListAppointmentDocumentQuery : IRequest<OperationResult<IList<KooliProjekt.Application.Data.AppointmentDocument>>>
    {
    }
}
