using System.Collections.Generic;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Doctor
{
    public class ListDoctorQuery : IRequest<OperationResult<IList<KooliProjekt.Application.Data.Doctor>>>
    {
    }
}
