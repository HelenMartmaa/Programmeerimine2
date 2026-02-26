using System.Collections.Generic;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Dto
{
    public class InvoiceRowDetailsDto
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public string ServiceDescription { get; set; }
        public decimal Fee { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
    }
}