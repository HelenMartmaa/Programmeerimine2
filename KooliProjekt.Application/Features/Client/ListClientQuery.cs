using System.Collections.Generic;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Client
{
    public class ListClientQuery : IRequest<OperationResult<IList<KooliProjekt.Application.Data.Client>>>
    {
    }
}
