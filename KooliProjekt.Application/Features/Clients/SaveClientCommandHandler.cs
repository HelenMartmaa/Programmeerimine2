using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Clients
{
    public class SaveClientCommandHandler : IRequestHandler<SaveClientCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveClientCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null) throw new ArgumentNullException(nameof(dbContext));
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveClientCommand request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var result = new OperationResult();
            if (request.ClientId < 0)
            {
                return result.AddPropertyError(nameof(request.ClientId), "Id cannot be negative.");
            }

            var client = new Client();
            if (request.ClientId == 0)
            {
                await _dbContext.Clients.AddAsync(client);
            }
            else
            {
                client = await _dbContext.Clients.FindAsync(request.ClientId);
                if (client == null)
                {
                    return result.AddPropertyError(nameof(request.ClientId), "Client with the specified Id does not exist.");
                }
            }

            client.UserId = request.UserId;
            client.PersonalCode = request.PersonalCode;
            client.DateOfBirth = request.DateOfBirth;
            client.Address = request.Address;

            await _dbContext.SaveChangesAsync();
            return result;
        }
    }
}