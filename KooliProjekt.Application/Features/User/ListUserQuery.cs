using System.Collections.Generic;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.User
{
    public class ListUserQuery : IRequest<OperationResult<IList<KooliProjekt.Application.Data.User>>>
    {
    }
}
