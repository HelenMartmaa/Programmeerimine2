using System;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Appointments
{
    public class SaveAppointmentCommandHandler : IRequestHandler<SaveAppointmentCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveAppointmentCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null) throw new ArgumentNullException(nameof(dbContext));
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveAppointmentCommand request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var result = new OperationResult();
            if (request.AppointmentId < 0)
            {
                return result.AddPropertyError(nameof(request.AppointmentId), "Id cannot be negative.");
            }

            var appointment = new Appointment();
            if (request.AppointmentId == 0)
            {
                await _dbContext.Appointments.AddAsync(appointment);
            }
            else
            {
                appointment = await _dbContext.Appointments.FindAsync(request.AppointmentId);
                if (appointment == null)
                {
                    return result.AddPropertyError(nameof(request.AppointmentId), "Appointment with the specified Id does not exist.");
                }
            }

            appointment.ClientId = request.ClientId;
            appointment.DoctorId = request.DoctorId;
            appointment.AppointmentDateTime = request.AppointmentDateTime;
            appointment.DurationMinutes = request.DurationMinutes;
            appointment.Status = request.Status;
            appointment.IsOutsideWorkingHours = request.IsOutsideWorkingHours;
            appointment.Notes = request.Notes;
            appointment.CreatedAt = request.CreatedAt;
            appointment.CancelledAt = request.CancelledAt;

            await _dbContext.SaveChangesAsync();
            return result;
        }
    }
}