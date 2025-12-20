using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Users
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteUserCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            // User can be Client, Doctor  or Administrator, the role needs to be deleted before deleting user

            // Delete if administrator
            await _dbContext.Administrators
                .Where(a => a.UserId == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            // Delete if client
            await _dbContext.Clients
                .Where(c => c.UserId == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            // Delete if doctor
            await _dbContext.Doctors
                .Where(d => d.UserId == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            // Delete User
            await _dbContext.Users
                .Where(u => u.UserId == request.Id)
                .ExecuteDeleteAsync(cancellationToken);

            return new OperationResult();
        }
    }
}