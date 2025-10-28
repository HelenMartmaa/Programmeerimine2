using System.Collections.Generic;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.DoctorUnavailability
{
    public class ListDoctorUnavailabilityQuery : IRequest<OperationResult<IList<KooliProjekt.Application.Data.DoctorUnavailability>>>
    {
    }
}
