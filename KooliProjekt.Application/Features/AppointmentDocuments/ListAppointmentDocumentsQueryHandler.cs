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

namespace KooliProjekt.Application.Features.AppointmentDocuments
{
    public class ListAppointmentDocumentsQueryHandler : IRequestHandler<ListAppointmentDocumentsQuery, OperationResult<PagedResult<AppointmentDocument>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListAppointmentDocumentsQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<AppointmentDocument>>> Handle(ListAppointmentDocumentsQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult<PagedResult<AppointmentDocument>>();

            if (request.Page <= 0 || request.PageSize <= 0)
            {
                return result;
            }

            var query = _dbContext.AppointmentDocuments.AsQueryable();

            if (!string.IsNullOrEmpty(request.DocumentType))
            {
                query = query.Where(d => d.DocumentType.Contains(request.DocumentType));
            }

            if (!string.IsNullOrEmpty(request.FileName))
            {
                query = query.Where(d => d.FileName.Contains(request.FileName));
            }

            result.Value = await query
                .OrderBy(d => d.DocumentId)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
