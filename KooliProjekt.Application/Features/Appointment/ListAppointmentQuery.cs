using System.Collections.Generic;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Appointment
{
    public class ListAppointmentQuery : IRequest<OperationResult<IList<KooliProjekt.Application.Data.Appointment>>>
    {
    }
}
