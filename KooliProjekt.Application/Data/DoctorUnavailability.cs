using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Application.Data
{
    public class DoctorUnavailability
    {
        [Key]
        public int UnavailabilityId { get; set; }
        public int DoctorId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }

        //Navigation property
        public Doctor Doctor { get; set; }

    }
}
