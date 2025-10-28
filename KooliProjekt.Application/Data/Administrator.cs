using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Application.Data
{
    public class Administrator
    {
        [Key]
        public int AdminId { get; set; }
        public int UserId { get; set; }
        public string Department { get; set; }

        //Navigation property
        public User User { get; set; }
    }
}
