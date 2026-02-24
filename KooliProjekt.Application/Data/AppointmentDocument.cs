using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    public class AppointmentDocument : Entity
    {
        [Key]
        public int DocumentId { get; set; }
        [Required] //Although int can't be nullable anyway
        [ForeignKey("Appointment")]
        public int AppointmentId { get; set; }
        [Required]
        [MaxLength(50)]
        public string DocumentType { get; set; }
        [Required]
        [MaxLength(50)]
        public string FileName { get; set; }
        [Required]
        [MaxLength(200)]
        public string FilePath { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.Now;
        [Range(1, 10485760)] //Currently max 10MB
        public long FileSize { get; set; }

        //Navigation property
        public Appointment Appointment { get; set; }

    }
}
