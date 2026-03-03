using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Clients
{
    public class GetClientQueryHandler : IRequestHandler<GetClientQuery, OperationResult<ClientDetailsDto>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetClientQueryHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }
            _dbContext = dbContext;
        }

        public async Task<OperationResult<ClientDetailsDto>> Handle(GetClientQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = new OperationResult<ClientDetailsDto>();

            if (request.Id <= 0)
            {
                return result;
            }

            result.Value = await _dbContext.Clients
                .Where(c => c.ClientId == request.Id)
                .Select(c => new ClientDetailsDto
                {
                    Id = c.ClientId,
                    UserId = c.UserId,
                    PersonalCode = c.PersonalCode,
                    DateOfBirth = c.DateOfBirth,
                    Address = c.Address
                })
                .FirstOrDefaultAsync();

            return result;
        }
    }
}