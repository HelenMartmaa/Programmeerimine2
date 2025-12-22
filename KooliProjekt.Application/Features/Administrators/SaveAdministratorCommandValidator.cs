using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Features.Administrators
{
    public class SaveAdministratorCommandValidator : AbstractValidator<SaveAdministratorCommand>
    {
        private readonly ApplicationDbContext _context;

        public SaveAdministratorCommandValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("User ID is required")
                .Custom((userId, context) =>
                {
                    var userExists = _context.Users.Any(u => u.UserId == userId);
                    if (!userExists)
                    {
                        context.AddFailure(nameof(SaveAdministratorCommand.UserId),
                            $"User with ID {userId} does not exist");
                    }
                });

            RuleFor(x => x.Department)
                .NotEmpty().WithMessage("Department is required")
                .MaximumLength(100).WithMessage("Department cannot exceed 100 characters");
        }
    }
}
