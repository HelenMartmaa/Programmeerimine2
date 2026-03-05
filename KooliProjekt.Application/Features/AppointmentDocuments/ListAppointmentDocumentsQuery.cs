using System;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.AppointmentDocuments
{
    [ExcludeFromCodeCoverage]
    public class ListAppointmentDocumentsQuery : IRequest<OperationResult<PagedResult<AppointmentDocument>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }

        public string DocumentType { get; set; }
        public string FileName { get; set; }
    }
}
