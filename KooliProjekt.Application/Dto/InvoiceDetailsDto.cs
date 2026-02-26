using System;
using System.Collections.Generic;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Dto
{
    public class InvoiceDetailsDto
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal TotalBeforeVat { get; set; }
        public decimal TotalWithVat { get; set; }
        public bool IsPaid { get; set; }
        public string InvoiceNum { get; set; }
        public IList<InvoiceRowDto> InvoiceRows { get; set; } = new List<InvoiceRowDto>();
    }
}