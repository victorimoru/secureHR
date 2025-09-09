using FluentValidation;

namespace SecureHR.Application.Features.Employee.Command
{
    public class HireEmployeeCommandValidator : AbstractValidator<HireEmployeeCommand>
    {
        public HireEmployeeCommandValidator()
        {
            RuleFor(v => v.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(100).WithMessage("First name must not exceed 100 characters.");

            RuleFor(v => v.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(100).WithMessage("Last name must not exceed 100 characters.");

            RuleFor(v => v.EmailAddress)
                .NotEmpty().WithMessage("Email address is required.")
                .EmailAddress().WithMessage("A valid email address is required.");

            RuleFor(v => v.DepartmentId)
                .NotEmpty().WithMessage("Department ID is required.");

            RuleFor(v => v.InitialSalary)
                .GreaterThan(0).WithMessage("Initial salary must be greater than zero.");
        }
    }
}
