using System.Collections.Generic;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Dto
{
    public class AppointmentDocumentDto
    {
        public int Id { get; set; }
        public string DocumentType { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }
    }
}