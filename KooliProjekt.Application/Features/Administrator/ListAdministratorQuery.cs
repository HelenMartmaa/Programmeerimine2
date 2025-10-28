using System.Collections.Generic;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Administrator
{
    public class ListAdministratorQuery : IRequest<OperationResult<IList<KooliProjekt.Application.Data.Administrator>>>
    {
    }
}
