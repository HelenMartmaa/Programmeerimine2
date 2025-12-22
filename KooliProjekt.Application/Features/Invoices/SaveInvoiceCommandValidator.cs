using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Features.Invoices
{
    public class SaveInvoiceCommandValidator : AbstractValidator<SaveInvoiceCommand>
    {
        private readonly ApplicationDbContext _context;

        public SaveInvoiceCommandValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.AppointmentId)
                .GreaterThan(0).WithMessage("Appointment ID is required")
                .Custom((appointmentId, context) =>
                {
                    var appointment = _context.Appointments.FirstOrDefault(a => a.AppointmentId == appointmentId);

                    if (appointment == null)
                    {
                        context.AddFailure(nameof(SaveInvoiceCommand.AppointmentId),
                            $"Appointment with ID {appointmentId} does not exist");
                    }
                    else if (appointment.Status != AppointmentStatus.Completed)
                    {
                        context.AddFailure(nameof(SaveInvoiceCommand.AppointmentId),
                            "Can only create invoice for completed appointments");
                    }
                });

            RuleFor(x => x.InvoiceDate)
                .NotEmpty().WithMessage("Invoice date is required")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Invoice date cannot be in the future");

            RuleFor(x => x.DueDate)
                .NotEmpty().WithMessage("Due date is required")
                .GreaterThanOrEqualTo(x => x.InvoiceDate).WithMessage("Due date cannot be earlier than invoice date")
                .LessThanOrEqualTo(x => x.InvoiceDate.AddDays(90)).WithMessage("Due date cannot be more than 90 days after invoice date");

            RuleFor(x => x.TotalBeforeVat)
                .GreaterThanOrEqualTo(0).WithMessage("Total before VAT cannot be negative")
                .LessThanOrEqualTo(999999.99m).WithMessage("Total before VAT is too large");

            RuleFor(x => x.TotalWithVat)
                .GreaterThanOrEqualTo(0).WithMessage("Total with VAT cannot be negative")
                .GreaterThanOrEqualTo(x => x.TotalBeforeVat).WithMessage("Total with VAT must be greater than or equal to total before VAT"); // if 0 then equal

            // Check if TotalWithVat = TotalBeforeVat * 1.22
            RuleFor(x => x)
                .Custom((command, context) =>
                {
                    var expectedWithVat = command.TotalBeforeVat * 1.22m;
                    var difference = Math.Abs(command.TotalWithVat - expectedWithVat);

                    if (difference > 0.01m) // Rounding error
                    {
                        context.AddFailure(nameof(command.TotalWithVat),
                            $"Total with VAT must equal Total before VAT * 1.22 (Expected: {expectedWithVat:F2})");
                    }
                });

            RuleFor(x => x.InvoiceNum)
                .NotEmpty().WithMessage("Invoice number is required")
                .MaximumLength(50).WithMessage("Invoice number cannot exceed 50 characters")
                .Custom((invoiceNum, context) =>
                {
                    var command = context.InstanceToValidate;

                    var existingInvoice = _context.Invoices
                        .Where(i => i.InvoiceNum == invoiceNum && i.InvoiceId != command.InvoiceId)
                        .Any();

                    if (existingInvoice)
                    {
                        context.AddFailure(nameof(command.InvoiceNum), "Invoice number already exists");
                    }
                });
        }
    }
}
