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
    public class ListDoctorsQueryHandler : IRequestHandler<ListDoctorsQuery, OperationResult<PagedResult<KooliProjekt.Application.Data.Doctor>>>
    {
        private readonly ApplicationDbContext _dbContext;
        public ListDoctorsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<PagedResult<KooliProjekt.Application.Data.Doctor>>> Handle(ListDoctorsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PagedResult<KooliProjekt.Application.Data.Doctor>>();
            result.Value = await _dbContext
                .Doctors
                .OrderBy(list => list.DoctorId)
                .GetPagedAsync(request.Page, request.PageSize);

            return result;
        }
    }
}
