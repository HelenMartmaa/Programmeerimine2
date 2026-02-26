using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Users
{
    public class SaveUserCommandHandler : IRequestHandler<SaveUserCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveUserCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null) throw new ArgumentNullException(nameof(dbContext));
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveUserCommand request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var result = new OperationResult();
            if (request.UserId < 0)
            {
                return result.AddPropertyError(nameof(request.UserId), "Id cannot be negative.");
            }

            var user = new User();
            if (request.UserId == 0)
            {
                await _dbContext.Users.AddAsync(user);
            }
            else
            {
                user = await _dbContext.Users.FindAsync(request.UserId);
                if (user == null)
                {
                    return result.AddPropertyError(nameof(request.UserId), "User with the specified Id does not exist.");
                }
            }

            user.Role = request.Role;
            user.Email = request.Email;
            user.PasswordHash = request.PasswordHash;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.PhoneNumber;
            user.CreatedAt = request.CreatedAt;

            await _dbContext.SaveChangesAsync();
            return result;
        }
    }
}