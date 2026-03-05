using System;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.DoctorUnavailabilities
{
    [ExcludeFromCodeCoverage]
    public class ListDoctorUnavailabilitiesQuery : IRequest<OperationResult<PagedResult<DoctorUnavailability>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }

        public int? DoctorId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
