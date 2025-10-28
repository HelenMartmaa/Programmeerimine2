using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Application.Data
{
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }
        public int UserId { get; set; }
        public string Specialization { get; set; }
        public string DocLicenseNum { get; set; }
        public TimeSpan WorkingHoursStart { get; set; } = new TimeSpan(8, 0, 0); //When assuming the clinic has working hours from 8am to 4pmS
        public TimeSpan WorkingHoursEnd { get; set; } = new TimeSpan(16, 0, 0);

        //Navigation properties
        public User User { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<DoctorUnavailability> Unavailabilities { get; set; }
    }
}
