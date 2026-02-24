using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    public class Client : Entity
    {
        [Key]
        public int ClientId { get; set; }
        [Required] //Although int can't be nullable anyway
        [ForeignKey("User")]
        public int UserId { get; set; }
        [Required]
        [MaxLength(11)]
        public string PersonalCode { get; set; }
        [Required]
        public DateTime? DateOfBirth { get; set; }
        [Required]
        [MaxLength(200)]
        public string Address { get; set; }

        //Navigation properties
        public User User { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}
