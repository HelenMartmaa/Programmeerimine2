using System.Collections.Generic;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.AppointmentDocuments
{
    public class ListAppointmentDocumentsQuery : IRequest<OperationResult<PagedResult<KooliProjekt.Application.Data.AppointmentDocument>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
