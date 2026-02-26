using System;
using System.Collections.Generic;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Dto
{
    public class AppointmentDetailsDto
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public int DurationMinutes { get; set; }
        public AppointmentStatus Status { get; set; }
        public string Notes { get; set; }
        public IList<AppointmentDocumentDto> Documents { get; set; } = new List<AppointmentDocumentDto>();
    }
}