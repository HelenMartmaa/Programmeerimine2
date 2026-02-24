using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace KooliProjekt.Application.Data
{
    public class Invoice : Entity
    {
        [Key]
        public int InvoiceId { get; set; }
        [Required] //Although int can't be nullable anyway
        [ForeignKey("Appointment")]
        public int AppointmentId { get; set; }
        public DateTime InvoiceDate { get; set; } = DateTime.Now;
        public DateTime DueDate { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        [Range(0, 999999.99)]
        public decimal TotalBeforeVat { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        [Range(0, 999999.99)]
        public decimal TotalWithVat { get; set; }
        public bool IsPaid { get; set; } = false;
        public DateTime? PaidAt { get; set; }
        [Required]
        [MaxLength(50)]
        public string InvoiceNum { get; set; }

        //Navigation properties
        public Appointment Appointment { get; set; }
        public ICollection<InvoiceRow> InvoiceRows { get; set; }

    }
}
