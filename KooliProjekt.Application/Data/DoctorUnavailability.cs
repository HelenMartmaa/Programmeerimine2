using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    public class DoctorUnavailability : Entity
    {
        [Key]
        public int UnavailabilityId { get; set; }

        public override int Id
        {
            get => UnavailabilityId;
            set => UnavailabilityId = value;
        }

        [Required] //Although int can't be nullable anyway
        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [MaxLength(200)]
        public string Reason { get; set; }

        //Navigation property
        public Doctor Doctor { get; set; }

    }
}
