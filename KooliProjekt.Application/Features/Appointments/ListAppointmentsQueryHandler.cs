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
    public class ListAppointmentsQueryHandler : IRequestHandler<ListAppointmentsQuery, OperationResult<IList<KooliProjekt.Application.Data.Appointment>>>
    {
        private readonly ApplicationDbContext _dbContext;
        public ListAppointmentsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<IList<KooliProjekt.Application.Data.Appointment>>> Handle(ListAppointmentsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<IList<KooliProjekt.Application.Data.Appointment>>();
            result.Value = await _dbContext
                .Appointments
                .OrderBy(r => r.AppointmentId)
                .ToListAsync(cancellationToken); //Allows the caller to cancel the ongoing query/operation

            return result;
        }
    }
}
