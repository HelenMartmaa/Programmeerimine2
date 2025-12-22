using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Features.Clients
{
    public class SaveClientCommandValidator : AbstractValidator<SaveClientCommand>
    {
        private readonly ApplicationDbContext _context;

        public SaveClientCommandValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("User ID is required")
                .Custom((userId, context) =>
                {
                    var userExists = _context.Users.Any(u => u.UserId == userId);
                    if (!userExists)
                    {
                        context.AddFailure(nameof(SaveClientCommand.UserId),
                            $"User with ID {userId} does not exist");
                    }
                });

            RuleFor(x => x.PersonalCode)
                .Length(11).WithMessage("Personal code must be exactly 11 characters")
                .Matches(@"^\d+$").WithMessage("Personal code must contain only digits")
                .When(x => !string.IsNullOrEmpty(x.PersonalCode));

            RuleFor(x => x.DateOfBirth)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Date of birth cannot be in the future")
                .GreaterThan(DateTime.Now.AddYears(-110)).WithMessage("Date of birth is too far in the past")
                .When(x => x.DateOfBirth.HasValue);

            RuleFor(x => x.Address)
                .MaximumLength(200).WithMessage("Address cannot exceed 200 characters")
                .When(x => !string.IsNullOrEmpty(x.Address));
        }
    }
}
