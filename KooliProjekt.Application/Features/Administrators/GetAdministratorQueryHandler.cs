using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Administrators
{
    // 28.11 - Kasutab IAdministratorRepository't
    public class GetAdministratorQueryHandler : IRequestHandler<GetAdministratorQuery, OperationResult<object>>
    {
        private readonly IAdministratorRepository _administratorRepository;

        public GetAdministratorQueryHandler(IAdministratorRepository administratorRepository)
        {
            _administratorRepository = administratorRepository;
        }

        public async Task<OperationResult<object>> Handle(GetAdministratorQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();
            var administrator = await _administratorRepository.GetByIdAsync(request.Id);

            result.Value = new
            {
                Id = administrator.AdminId,
                UserId = administrator.UserId,
                Department = administrator.Department,
                User = administrator.User != null ? new
                {
                    Id = administrator.User.UserId,
                    FirstName = administrator.User.FirstName,
                    LastName = administrator.User.LastName,
                    Email = administrator.User.Email
                } : null
            };

            return result;
        }
    }
}