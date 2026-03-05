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

namespace KooliProjekt.Application.Features.Doctors
{
    public class ListDoctorsQueryHandler : IRequestHandler<ListDoctorsQuery, OperationResult<PagedResult<Doctor>>>
    {
        private readonly ApplicationDbContext _dbContext;

        public ListDoctorsQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<Doctor>>> Handle(ListDoctorsQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult<PagedResult<Doctor>>();

            if (request.Page <= 0 || request.PageSize <= 0)
            {
                return result;
            }

            var query = _dbContext.Doctors.AsQueryable();

            if (!string.IsNullOrEmpty(request.Specialization))
            {
                query = query.Where(d => d.Specialization.Contains(request.Specialization));
            }

            if (!string.IsNullOrEmpty(request.DocLicenseNum))
            {
                query = query.Where(d => d.DocLicenseNum.Contains(request.DocLicenseNum));
            }

            result.Value = await query
                .OrderBy(d => d.DoctorId)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}