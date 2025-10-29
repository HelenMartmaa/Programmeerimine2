using System.Collections.Generic;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Clients
{
    public class ListClientsQuery : IRequest<OperationResult<IList<KooliProjekt.Application.Data.Client>>>
    {
    }
}
