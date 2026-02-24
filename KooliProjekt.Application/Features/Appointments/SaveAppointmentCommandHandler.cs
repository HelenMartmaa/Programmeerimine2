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
    public class SaveAppointmentCommandHandler : IRequestHandler<SaveAppointmentCommand, OperationResult>
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public SaveAppointmentCommandHandler(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<OperationResult> Handle(SaveAppointmentCommand request, CancellationToken cancellationToken)
        {
            var appointment = new Appointment();
            
            if (request.AppointmentId != 0)
            {
                appointment = await _appointmentRepository.GetByIdAsync(request.AppointmentId);
            }

            appointment.ClientId = request.ClientId;
            appointment.DoctorId = request.DoctorId;
            appointment.AppointmentDateTime = request.AppointmentDateTime;
            appointment.DurationMinutes = request.DurationMinutes;
            appointment.Status = request.Status;
            appointment.IsOutsideWorkingHours = request.IsOutsideWorkingHours;
            appointment.Notes = request.Notes;
            appointment.CancelledAt = request.CancelledAt;
            
            if (request.AppointmentId == 0)
            {
                appointment.CreatedAt = request.CreatedAt == default ? System.DateTime.Now : request.CreatedAt;
            }

            await _appointmentRepository.SaveAsync(appointment);

            return new OperationResult();
        }
    }
}