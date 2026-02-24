using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Appointments
{
    public class GetAppointmentQueryHandler : IRequestHandler<GetAppointmentQuery, OperationResult<object>>
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public GetAppointmentQueryHandler(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<OperationResult<object>> Handle(GetAppointmentQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();
            var appointment = await _appointmentRepository.GetByIdAsync(request.Id);

            result.Value = new
            {
                Id = appointment.AppointmentId,
                ClientId = appointment.ClientId,
                DoctorId = appointment.DoctorId,
                AppointmentDateTime = appointment.AppointmentDateTime,
                DurationMinutes = appointment.DurationMinutes,
                Status = appointment.Status,
                IsOutsideWorkingHours = appointment.IsOutsideWorkingHours,
                Notes = appointment.Notes,
                CreatedAt = appointment.CreatedAt,
                CancelledAt = appointment.CancelledAt,
                Client = appointment.Client != null ? new
                {
                    Id = appointment.Client.ClientId,
                    Name = $"{appointment.Client.User.FirstName} {appointment.Client.User.LastName}"
                } : null,
                Doctor = appointment.Doctor != null ? new
                {
                    Id = appointment.Doctor.DoctorId,
                    Name = $"{appointment.Doctor.User.FirstName} {appointment.Doctor.User.LastName}",
                    Specialization = appointment.Doctor.Specialization
                } : null,
                Documents = appointment.Documents?.Select(d => new
                {
                    Id = d.DocumentId,
                    DocumentType = d.DocumentType,
                    FileName = d.FileName
                })
            };

            return result;
        }
    }
}