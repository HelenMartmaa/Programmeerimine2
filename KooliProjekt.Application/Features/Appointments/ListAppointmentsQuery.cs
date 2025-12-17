using System.Collections.Generic;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Appointments
{
    public class ListAppointmentsQuery : IRequest<OperationResult<PagedResult<KooliProjekt.Application.Data.Appointment>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
