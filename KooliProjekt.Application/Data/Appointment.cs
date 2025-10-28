using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }
        [Required] //Although int can't be nullable anyway
        [ForeignKey("Client")]
        public int ClientId { get; set; }
        [Required] //Although int can't be nullable anyway
        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public int DurationMinutes { get; set; } = 30; //Default appointment duration that will be used in booking system, but administrator will have possibility to update it?
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled; //Default value is "Scheduled"
        public bool IsOutsideWorkingHours { get; set; } = false; //By default the appointment will be inside working hours (only admin can put appointments outside working hours)
        [MaxLength(500)]
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? CancelledAt { get; set; } //Nullable (only when appointment is being cancelled

        //Navigation properties
        public Client Client { get; set; }
        public Doctor Doctor { get; set; }
        public Invoice Invoice { get; set; }
        public ICollection<AppointmentDocument> Documents { get; set; }

    }
}
