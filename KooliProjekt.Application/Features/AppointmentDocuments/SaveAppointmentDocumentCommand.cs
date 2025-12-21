using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.AppointmentDocuments
{
    public class SaveAppointmentDocumentCommand : IRequest<OperationResult>, ITransactional
    {
        public int DocumentId { get; set; }
        public int AppointmentId { get; set; }
        public string DocumentType { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadedAt { get; set; }
        public long FileSize { get; set; }
    }
}
