using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Administrators
{
    public class GetAdministratorQueryHandler : IRequestHandler<GetAdministratorQuery, OperationResult<AdministratorDetailsDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetAdministratorQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }
            _dbContext = dbContext;
        }

        public async Task<OperationResult<AdministratorDetailsDto>> Handle(GetAdministratorQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult<AdministratorDetailsDto>();

            if (request.Id <= 0)
            {
                return result;
            }

            result.Value = await _dbContext.Administrators
                .Where(a => a.AdminId == request.Id)
                .Select(a => new AdministratorDetailsDto
                {
                    Id = a.AdminId,
                    UserId = a.UserId,
                    Department = a.Department
                })
                .FirstOrDefaultAsync();

            return result;
        }
    }
}