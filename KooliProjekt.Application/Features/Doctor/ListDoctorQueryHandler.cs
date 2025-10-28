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

namespace KooliProjekt.Application.Features.Doctor
{
    public class ListDoctorQueryHandler : IRequestHandler<ListDoctorQuery, OperationResult<IList<KooliProjekt.Application.Data.Doctor>>>
    {
        private readonly ApplicationDbContext _dbContext;
        public ListDoctorQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<IList<KooliProjekt.Application.Data.Doctor>>> Handle(ListDoctorQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<IList<KooliProjekt.Application.Data.Doctor>>();
            result.Value = await _dbContext
                .Doctors
                .OrderBy(r => r.DoctorId)
                .ToListAsync(cancellationToken); //Allows the caller to cancel the ongoing query/operation

            return result;
        }
    }
}
