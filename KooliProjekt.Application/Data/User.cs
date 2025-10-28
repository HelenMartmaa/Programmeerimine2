using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public UserRole Role { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        //Navigation properties
        public Client Client { get; set; }
        public Doctor Doctor { get; set; }
        public Administrator Administrator { get; set; }
    }
}
