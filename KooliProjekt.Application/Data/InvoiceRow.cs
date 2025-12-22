using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    public class InvoiceRow
    {
        [Key]
        public int InvoiceRowId { get; set; }
        [Required] //Although int can't be nullable anyway
        [ForeignKey("Invoice")]
        public int InvoiceId { get; set; }
        [Required]
        [MaxLength(200)]
        [MinLength(3)]
        public string ServiceDescription { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        [Range(0, 10000)]
        public decimal Fee { get; set; }
        [Range(1, 100)]
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        [Range(0, 99999)]
        public decimal Discount { get; set; }

        //Navigation property
        public Invoice Invoice { get; set; }
    }
}
