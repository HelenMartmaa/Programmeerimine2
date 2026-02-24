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
    public class SaveAdministratorCommandHandler : IRequestHandler<SaveAdministratorCommand, OperationResult>
    {
        private readonly IAdministratorRepository _administratorRepository;

        public SaveAdministratorCommandHandler(IAdministratorRepository administratorRepository)
        {
            _administratorRepository = administratorRepository;
        }

        public async Task<OperationResult> Handle(SaveAdministratorCommand request, CancellationToken cancellationToken)
        {
            var administrator = new Administrator();
            
            if (request.AdminId != 0)
            {
                // UPDATE - leia olemasolev
                administrator = await _administratorRepository.GetByIdAsync(request.AdminId);
            }

            // Muuda -> properties
            administrator.UserId = request.UserId;
            administrator.Department = request.Department;

            // Salvesta (INSERT või UPDATE)
            await _administratorRepository.SaveAsync(administrator);

            return new OperationResult();
        }
    }
}