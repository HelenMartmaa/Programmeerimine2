using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Features.InvoiceRows
{
    public class SaveInvoiceRowCommandValidator : AbstractValidator<SaveInvoiceRowCommand>
    {
        private readonly ApplicationDbContext _context;

        public SaveInvoiceRowCommandValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.InvoiceId)
                .GreaterThan(0).WithMessage("Invoice ID is required")
                .Custom((invoiceId, context) =>
                {
                    var invoiceExists = _context.Invoices.Any(i => i.InvoiceId == invoiceId);
                    if (!invoiceExists)
                    {
                        context.AddFailure(nameof(SaveInvoiceRowCommand.InvoiceId),
                            $"Invoice with ID {invoiceId} does not exist");
                    }
                });

            RuleFor(x => x.ServiceDescription)
                .NotEmpty().WithMessage("Service description is required")
                .MinimumLength(3).WithMessage("Service description must be at least 3 characters")
                .MaximumLength(200).WithMessage("Service description cannot exceed 200 characters");

            RuleFor(x => x.Fee)
                .GreaterThanOrEqualTo(0).WithMessage("Fee cannot be negative")
                .LessThanOrEqualTo(10000).WithMessage("Fee cannot exceed 10,000");

            RuleFor(x => x.Quantity)
                .InclusiveBetween(1, 50).WithMessage("Quantity must be between 1 and 50");

            RuleFor(x => x.Discount)
                .GreaterThanOrEqualTo(0).WithMessage("Discount cannot be negative")
                .LessThan(x => x.Fee).WithMessage("Discount cannot be greater than or equal to fee");
        }
    }
}
