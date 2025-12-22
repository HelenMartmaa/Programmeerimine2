using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Features.AppointmentDocuments
{
    public class SaveAppointmentDocumentCommandValidator : AbstractValidator<SaveAppointmentDocumentCommand>
    {
        private readonly ApplicationDbContext _context;
        private readonly string[] _allowedDocumentTypes = { "X-Ray", "Lab Result", "Medical Report", "Prescription", "Referral" };

        public SaveAppointmentDocumentCommandValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.AppointmentId)
                .GreaterThan(0).WithMessage("Appointment ID is required")
                .Custom((appointmentId, context) =>
                {
                    var appointmentExists = _context.Appointments.Any(a => a.AppointmentId == appointmentId);
                    if (!appointmentExists)
                    {
                        context.AddFailure(nameof(SaveAppointmentDocumentCommand.AppointmentId),
                            $"Appointment with ID {appointmentId} does not exist");
                    }
                });

            RuleFor(x => x.DocumentType)
                .NotEmpty().WithMessage("Document type is required")
                .MaximumLength(50).WithMessage("Document type cannot exceed 50 characters")
                .Must(x => _allowedDocumentTypes.Contains(x)).WithMessage($"Document type must be one of: {string.Join(", ", _allowedDocumentTypes)}");

            RuleFor(x => x.FileName)
                .NotEmpty().WithMessage("File name is required")
                .MaximumLength(255).WithMessage("File name cannot exceed 255 characters");

            RuleFor(x => x.FilePath)
                .NotEmpty().WithMessage("File path is required")
                .MaximumLength(500).WithMessage("File path cannot exceed 500 characters");

            RuleFor(x => x.FileSize)
                .InclusiveBetween(1, 10485760).WithMessage("File size must be between 1 byte and 10 MB");
        }
    }
}
