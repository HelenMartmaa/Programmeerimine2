using System;
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
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            // User can be Client, Doctor or Administrator, the role needs to be deleted before deleting user
            var result = new OperationResult();

            if (request.Id <= 0)
            {
                return result;
            }

            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.UserId == request.Id);

            if (user == null)
            {
                return result;
            }

            // Delete if administrator
            var administrator = await _dbContext.Administrators
                .FirstOrDefaultAsync(a => a.UserId == request.Id);
            if (administrator != null)
            {
                _dbContext.Administrators.Remove(administrator);
            }

            // Delete if client
            var client = await _dbContext.Clients
                .FirstOrDefaultAsync(c => c.UserId == request.Id);
            if (client != null)
            {
                _dbContext.Clients.Remove(client);
            }

            // Delete if doctor
            var doctor = await _dbContext.Doctors
                .FirstOrDefaultAsync(d => d.UserId == request.Id);
            if (doctor != null)
            {
                _dbContext.Doctors.Remove(doctor);
            }

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}