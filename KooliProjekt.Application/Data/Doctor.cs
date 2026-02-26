using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    public class Doctor : Entity
    {
        [Key]
        public int DoctorId { get; set; }

        public override int Id
        {
            get => DoctorId;
            set => DoctorId = value;
        }

        [Required] //Although int can't be nullable anyway
        [ForeignKey("User")]
        public int UserId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Specialization { get; set; }
        [Required]
        [MaxLength(50)]
        public string DocLicenseNum { get; set; }
        public TimeSpan WorkingHoursStart { get; set; } = new TimeSpan(8, 0, 0); //When assuming the clinic has working hours from 8am to 4pm
        public TimeSpan WorkingHoursEnd { get; set; } = new TimeSpan(16, 0, 0);

        //Navigation properties
        public User User { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<DoctorUnavailability> Unavailabilities { get; set; }
    }
}
