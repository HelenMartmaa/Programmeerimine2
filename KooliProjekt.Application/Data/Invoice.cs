using Microsoft.VisualBasic;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace KooliProjekt.Application.Data
{
    public class Invoice
    {
        [Key]
        public int InvoiceId { get; set; }
        public int AppointmentId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalBeforeVat { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalWithVat { get; set; }
        public bool IsPaid { get; set; }
        public DateTime? PaidAt { get; set; } //Nullable (only when is paid)
        public string InvoiceNum { get; set; }

        //Navigation properties
        public Appointment Appointment { get; set; }
        public ICollection<InvoiceRow> InvoiceRows { get; set; }

    }
}
