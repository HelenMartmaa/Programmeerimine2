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

namespace KooliProjekt.Application.Features.AppointmentDocument
{
    public class ListAppointmentDocumentQueryHandler : IRequestHandler<ListAppointmentDocumentQuery, OperationResult<IList<KooliProjekt.Application.Data.AppointmentDocument>>>
    {
        private readonly ApplicationDbContext _dbContext;
        public ListAppointmentDocumentQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<IList<KooliProjekt.Application.Data.AppointmentDocument>>> Handle(ListAppointmentDocumentQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<IList<KooliProjekt.Application.Data.AppointmentDocument>>();
            result.Value = await _dbContext
                .AppointmentDocuments
                .OrderBy(r => r.DocumentId)
                .ToListAsync(cancellationToken); //Allows the caller to cancel the ongoing query/operation

            return result;
        }
    }
}
