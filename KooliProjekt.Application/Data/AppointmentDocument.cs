using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Application.Data
{
    public class AppointmentDocument
    {
        [Key]
        public int DocumentId { get; set; }
        public int AppointmentId { get; set; }
        public string DocumentType { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.Now;
        public long FileSize { get; set; }

        //Navigation property
        public Appointment Appointment { get; set; }

    }
}
