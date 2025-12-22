using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Features.Doctors
{
    public class SaveDoctorCommandValidator : AbstractValidator<SaveDoctorCommand>
    {
        private readonly ApplicationDbContext _context;

        public SaveDoctorCommandValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("User ID is required")
                .Custom((userId, context) =>
                {
                    var userExists = _context.Users.Any(u => u.UserId == userId);
                    if (!userExists)
                    {
                        context.AddFailure(nameof(SaveDoctorCommand.UserId),
                            $"User with ID {userId} does not exist");
                    }
                });

            RuleFor(x => x.Specialization)
                .NotEmpty().WithMessage("Specialization is required")
                .MaximumLength(100).WithMessage("Specialization cannot exceed 100 characters");

            RuleFor(x => x.DocLicenseNum)
                .MaximumLength(50).WithMessage("License number cannot exceed 50 characters")
                .When(x => !string.IsNullOrEmpty(x.DocLicenseNum));

            RuleFor(x => x.WorkingHoursStart)
                .NotEmpty().WithMessage("Working hours start is required")
                .GreaterThanOrEqualTo(new TimeSpan(6, 0, 0)).WithMessage("Working hours cannot start before 6:00 AM");
                //Optional rule for the work start time restrictions
                //.LessThan(new TimeSpan(12, 0, 0)).WithMessage("Working hours must start before 12:00 PM"); 

            RuleFor(x => x.WorkingHoursEnd)
                .NotEmpty().WithMessage("Working hours end is required")
                //Optional rule for the work end time restrictions
                //.GreaterThan(new TimeSpan(14, 0, 0)).WithMessage("Working hours must end after 2:00 PM") 
                .LessThanOrEqualTo(new TimeSpan(20, 0, 0)).WithMessage("Working hours cannot end after 8:00 PM");

            RuleFor(x => x)
                .Custom((command, context) =>
                {
                    if (command.WorkingHoursEnd <= command.WorkingHoursStart)
                    {
                        context.AddFailure(nameof(command.WorkingHoursEnd),
                            "Working hours end must be after working hours start");
                    }
                });
        }
    }
}
