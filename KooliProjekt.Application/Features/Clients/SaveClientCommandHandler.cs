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

namespace KooliProjekt.Application.Features.Clients
{
    public class SaveClientCommandHandler : IRequestHandler<SaveClientCommand, OperationResult>
    {
        private readonly IClientRepository _clientRepository;

        public SaveClientCommandHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<OperationResult> Handle(SaveClientCommand request, CancellationToken cancellationToken)
        {
            var client = new Client();
            
            if (request.ClientId != 0)
            {
                client = await _clientRepository.GetByIdAsync(request.ClientId);
            }

            client.UserId = request.UserId;
            client.PersonalCode = request.PersonalCode;
            client.DateOfBirth = request.DateOfBirth;
            client.Address = request.Address;

            await _clientRepository.SaveAsync(client);

            return new OperationResult();
        }
    }
}