using System.Collections.Generic;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.AppointmentDocuments
{
    public class ListAppointmentDocumentsQuery : IRequest<OperationResult<IList<KooliProjekt.Application.Data.AppointmentDocument>>>
    {
    }
}
