using System;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Doctors
{
    [ExcludeFromCodeCoverage]
    public class ListDoctorsQuery : IRequest<OperationResult<PagedResult<Doctor>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }

        public string Specialization { get; set; }
        public string DocLicenseNum { get; set; }
    }
}
