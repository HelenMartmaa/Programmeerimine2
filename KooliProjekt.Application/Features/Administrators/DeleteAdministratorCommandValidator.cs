using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Features.Administrators
{
    // Not neccessary validation, but added just for testing
    public class DeleteAdministratorCommandValidator : AbstractValidator<DeleteAdministratorCommand>
    {
        private readonly ApplicationDbContext _context;

        public DeleteAdministratorCommandValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("ID must be greater than 0")
                .Custom((id, context) =>
                {
                    var exists = _context.Administrators.Any(a => a.AdminId == id);
                    if (!exists)
                    {
                        context.AddFailure(nameof(DeleteAdministratorCommand.Id),
                            $"Administrator with ID {id} does not exist");
                    }
                });
        }
    }
}
