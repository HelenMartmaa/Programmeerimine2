using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Application.Data
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }
        public int ClientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public int DurationMinutes { get; set; } = 30; //Default appointment duration that will be used in booking system
        public AppointmentStatus Status { get; set; }
        public bool IsOutsideWorkingHours { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CancelledAt { get; set; } //Nullable (only when appointment is being cancelled

        //Navigation properties
        public Client Client { get; set; }
        public Doctor Doctor { get; set; }
        public Invoice Invoice { get; set; }
        public ICollection<AppointmentDocument> Documents { get; set; }

    }
}
