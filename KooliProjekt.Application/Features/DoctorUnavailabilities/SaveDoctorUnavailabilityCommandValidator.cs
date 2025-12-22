using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Features.DoctorUnavailabilities
{
    public class SaveDoctorUnavailabilityCommandValidator : AbstractValidator<SaveDoctorUnavailabilityCommand>
    {
        private readonly ApplicationDbContext _context;

        public SaveDoctorUnavailabilityCommandValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.DoctorId)
                .GreaterThan(0).WithMessage("Doctor ID is required")
                .Custom((doctorId, context) =>
                {
                    var doctorExists = _context.Doctors.Any(d => d.DoctorId == doctorId);
                    if (!doctorExists)
                    {
                        context.AddFailure(nameof(SaveDoctorUnavailabilityCommand.DoctorId),
                            $"Doctor with ID {doctorId} does not exist");
                    }
                });

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start date is required")
                .GreaterThanOrEqualTo(DateTime.Now.Date).WithMessage("Start date cannot be in the past");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("End date is required")
                .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date")
                .LessThanOrEqualTo(x => x.StartDate.AddYears(1)).WithMessage("Unavailability period cannot exceed 1 year"); // Can't be added with data annotations

            RuleFor(x => x.Reason)
                .MaximumLength(200).WithMessage("Reason cannot exceed 200 characters")
                .When(x => !string.IsNullOrEmpty(x.Reason));
        }
    }
}
