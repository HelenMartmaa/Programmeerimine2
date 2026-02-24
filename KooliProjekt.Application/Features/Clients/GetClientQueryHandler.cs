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
    public class GetClientQueryHandler : IRequestHandler<GetClientQuery, OperationResult<object>>
    {
        private readonly IClientRepository _clientRepository;

        public GetClientQueryHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<OperationResult<object>> Handle(GetClientQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();
            var client = await _clientRepository.GetByIdAsync(request.Id);

            result.Value = new
            {
                Id = client.ClientId,
                UserId = client.UserId,
                PersonalCode = client.PersonalCode,
                DateOfBirth = client.DateOfBirth,
                Address = client.Address,
                User = client.User != null ? new
                {
                    Id = client.User.UserId,
                    FirstName = client.User.FirstName,
                    LastName = client.User.LastName,
                    Email = client.User.Email
                } : null,
                Appointments = client.Appointments?.Select(a => new
                {
                    Id = a.AppointmentId,
                    AppointmentDateTime = a.AppointmentDateTime,
                    Status = a.Status
                })
            };

            return result;
        }
    }
}