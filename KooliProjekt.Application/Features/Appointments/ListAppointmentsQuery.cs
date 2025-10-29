using System.Collections.Generic;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Appointments
{
    public class ListAppointmentsQuery : IRequest<OperationResult<IList<KooliProjekt.Application.Data.Appointment>>>
    {
    }
}
