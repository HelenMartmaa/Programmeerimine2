using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Administrators
{
    public class SaveAdministratorCommandHandler : IRequestHandler<SaveAdministratorCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveAdministratorCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null) throw new ArgumentNullException(nameof(dbContext));
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveAdministratorCommand request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var result = new OperationResult();
            if (request.AdminId < 0)
            {
                return result.AddPropertyError(nameof(request.AdminId), "Id cannot be negative.");
            }

            var administrator = new Administrator();
            if (request.AdminId == 0)
            {
                await _dbContext.Administrators.AddAsync(administrator);
            }
            else
            {
                administrator = await _dbContext.Administrators.FindAsync(request.AdminId);
                if (administrator == null)
                {
                    return result.AddPropertyError(nameof(request.AdminId), "Administrator with the specified Id does not exist.");
                }
            }

            administrator.UserId = request.UserId;
            administrator.Department = request.Department;

            await _dbContext.SaveChangesAsync();
            return result;
        }
    }
}