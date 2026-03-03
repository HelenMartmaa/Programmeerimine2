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
    public class ListAppointmentsQueryHandler : IRequestHandler<ListAppointmentsQuery, OperationResult<PagedResult<Appointment>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListAppointmentsQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<Appointment>>> Handle(ListAppointmentsQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult<PagedResult<Appointment>>();

            if (request.Page <= 0 || request.PageSize <= 0)
            {
                return result;
            }

            result.Value = await _dbContext
                .Appointments
                .OrderBy(a => a.AppointmentId)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
