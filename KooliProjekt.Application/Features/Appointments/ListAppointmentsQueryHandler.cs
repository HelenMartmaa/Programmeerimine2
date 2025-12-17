using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Appointments
{
    public class ListAppointmentsQueryHandler : IRequestHandler<ListAppointmentsQuery, OperationResult<PagedResult<KooliProjekt.Application.Data.Appointment>>>
    {
        private readonly ApplicationDbContext _dbContext;
        public ListAppointmentsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<KooliProjekt.Application.Data.Appointment>>> Handle(ListAppointmentsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<KooliProjekt.Application.Data.Appointment>>();
            result.Value = await _dbContext
                .Appointments
                .OrderBy(list => list.AppointmentId)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
