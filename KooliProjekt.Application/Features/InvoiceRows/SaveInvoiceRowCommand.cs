using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.InvoiceRows
{
    public class SaveInvoiceRowCommand : IRequest<OperationResult>, ITransactional
    {
        public int InvoiceRowId { get; set; }
        public int InvoiceId { get; set; }
        public string ServiceDescription { get; set; }
        public decimal Fee { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
    }
}
