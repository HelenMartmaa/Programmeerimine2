using System;
using System.Collections.Generic;
using FluentValidation;
using KooliProjekt.Application.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Appointments
{
    public class SaveAppointmentCommandValidator : AbstractValidator<SaveAppointmentCommand>
    {
        private readonly ApplicationDbContext _context;

        public SaveAppointmentCommandValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.ClientId)
                .GreaterThan(0).WithMessage("Client ID is required")
                .Custom((clientId, context) =>
                {
                    var clientExists = _context.Clients.Any(c => c.ClientId == clientId);
                    if (!clientExists)
                    {
                        context.AddFailure(nameof(SaveAppointmentCommand.ClientId),
                            $"Client with ID {clientId} does not exist");
                    }
                });

            RuleFor(x => x.DoctorId)
                .GreaterThan(0).WithMessage("Doctor ID is required")
                .Custom((doctorId, context) =>
                {
                    var doctorExists = _context.Doctors.Any(d => d.DoctorId == doctorId);
                    if (!doctorExists)
                    {
                        context.AddFailure(nameof(SaveAppointmentCommand.DoctorId),
                            $"Doctor with ID {doctorId} does not exist");
                    }
                });

            RuleFor(x => x.AppointmentDateTime)
                .NotEmpty().WithMessage("Appointment date and time is required")
                .GreaterThanOrEqualTo(DateTime.Now).WithMessage("Appointment cannot be in the past")
                .LessThanOrEqualTo(DateTime.Now.AddDays(30)).WithMessage("Cannot book more than 30 days in advance")
                .When(x => !x.IsOutsideWorkingHours); // Only admin has the possibility to make an appointment into past - this feature still inc

            RuleFor(x => x.DurationMinutes)
                .InclusiveBetween(15, 120).WithMessage("Appointment duration must be between 15 and 120 minutes"); // Default time 30 min

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid appointment status");

            RuleFor(x => x.Notes)
                .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters")
                .When(x => !string.IsNullOrEmpty(x.Notes));

            // Double-booking control
            RuleFor(x => x)
                .Custom((command, context) =>
                {
                    var appointmentEnd = command.AppointmentDateTime.AddMinutes(command.DurationMinutes);

                    // Check if doctor is available
                    var doctorConflict = _context.Appointments
                        .Where(a => a.DoctorId == command.DoctorId)
                        .Where(a => a.AppointmentId != command.AppointmentId) // Unless updating own appointment
                        .Where(a => a.Status == AppointmentStatus.Scheduled)
                        .Any(a => a.AppointmentDateTime < appointmentEnd &&
                                  a.AppointmentDateTime.AddMinutes(a.DurationMinutes) > command.AppointmentDateTime);

                    if (doctorConflict)
                    {
                        context.AddFailure(nameof(command.AppointmentDateTime),
                            "Doctor is already booked at this time");
                    }

                    // Check if client has some other appointment at the same time
                    var clientConflict = _context.Appointments
                        .Where(a => a.ClientId == command.ClientId)
                        .Where(a => a.AppointmentId != command.AppointmentId)
                        .Where(a => a.Status == AppointmentStatus.Scheduled)
                        .Any(a => a.AppointmentDateTime < appointmentEnd &&
                                  a.AppointmentDateTime.AddMinutes(a.DurationMinutes) > command.AppointmentDateTime);

                    if (clientConflict)
                    {
                        context.AddFailure(nameof(command.AppointmentDateTime),
                            "Client already has an appointment at this time");
                    }
                });
        }
    }
}
