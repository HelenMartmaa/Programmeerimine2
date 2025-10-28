using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Application.Data
{
    public class Client
    {
        [Key]
        public int ClientId { get; set; }
        public int UserId { get; set; }
        public string PersonalCode { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Address { get; set; }

        //Navigation properties
        public User User { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}
