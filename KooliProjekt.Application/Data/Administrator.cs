using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    public class Administrator : Entity
    {
        [Key]
        public int AdminId { get; set; }
        [Required] //Although int can't be nullable anyway
        [ForeignKey("User")]
        public int UserId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Department { get; set; } //Nullable

        //Navigation property
        public User User { get; set; }
    }
}
