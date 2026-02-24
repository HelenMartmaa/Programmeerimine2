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

namespace KooliProjekt.Application.Features.Users
{
    public class SaveUserCommandHandler : IRequestHandler<SaveUserCommand, OperationResult>
    {
        private readonly IUserRepository _userRepository;

        public SaveUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<OperationResult> Handle(SaveUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User();
            
            if (request.UserId != 0)
            {
                user = await _userRepository.GetByIdAsync(request.UserId);
            }

            user.Role = request.Role;
            user.Email = request.Email;
            user.PasswordHash = request.PasswordHash;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.PhoneNumber;
            
            if (request.UserId == 0)
            {
                // INSERT - määra CreatedAt ainult uue jaoks
                user.CreatedAt = request.CreatedAt == default ? System.DateTime.Now : request.CreatedAt;
            }

            await _userRepository.SaveAsync(user);

            return new OperationResult();
        }
    }
}