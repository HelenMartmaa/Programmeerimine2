using FluentValidation;
using KooliProjekt.Application.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace KooliProjekt.Application.Features.Users
{
    public class SaveUserCommandValidator : AbstractValidator<SaveUserCommand>
    {
        private readonly ApplicationDbContext _context;

        public SaveUserCommandValidator(ApplicationDbContext context)
        {
            _context = context;

            // RUle for email
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters")
                .EmailAddress().WithMessage("Invalid email format")
                .Custom((email, context) =>
                {
                    var command = context.InstanceToValidate;

                    // To control, if the email is already existing
                    var existingUser = _context.Users
                        .Where(u => u.Email == email && u.UserId != command.UserId)
                        .Any();

                    if (existingUser)
                    {
                        context.AddFailure(nameof(command.Email), "Email already exists");
                    }
                });

            // Rule for password
            RuleFor(x => x.PasswordHash)
                .NotEmpty().WithMessage("Password is required")
                .MaximumLength(255).WithMessage("Password cannot exceed 100 characters");

            // Rule for first name
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MinimumLength(2).WithMessage("First name must be at least 2 characters")
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters");

            // Rule for last name
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MinimumLength(2).WithMessage("Last name must be at least 2 characters")
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters");

            // Rule for phone number
            RuleFor(x => x.PhoneNumber)
                .MaximumLength(20).WithMessage("Phone number cannot exceed 20 characters")
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

            // Rule for user role
            RuleFor(x => x.Role)
                .IsInEnum().WithMessage("Invalid user role");
        }
    }
}