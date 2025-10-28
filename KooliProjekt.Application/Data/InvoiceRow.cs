using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KooliProjekt.Application.Data
{
    public class InvoiceRow
    {
        [Key]
        public int InvoiceRowId { get; set; }
        public int InvoiceId { get; set; }
        public string ServiceDescription { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal Fee { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal Discount { get; set; }

        //Navigation property
        public Invoice Invoice { get; set; }
    }
}
