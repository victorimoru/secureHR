using MediatR;

namespace SecureHR.Application.Features.Employee.Command
{
    public record HireEmployeeCommand(
        string FirstName,
        string LastName,
        string Title,
        Guid DepartmentId,
        string Street,
        string City,
        string PostalCode,
        string PhoneNumber,
        decimal InitialSalary,
        string HireReason,
        string EmailAddress,
        Guid Id
    ) : IRequest<Guid>
    {
        public Guid IdempotencyKey { get; set; }
    }

    public record EmployeeDto(
        Guid Id,
        string FullName,
        Guid DepartmentId,
        string Status,
        DateTime HireDate,
        decimal CurrentSalary
    );
}
