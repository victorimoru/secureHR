using MediatR;
using SecureHR.Core.Domains.EmployeeAggregate;
using SecureHR.Core.Repositories;

namespace SecureHR.Application.Features.Employee.Command
{
    public class HireEmployeeCommandHandler(IIdempotencyRepository idempotencyRepository) : IRequestHandler<HireEmployeeCommand, Guid>
    {
        public async Task<Guid> Handle(HireEmployeeCommand request, CancellationToken cancellationToken)
        {
            var IsRequestAlreadyProcessed = await idempotencyRepository.KeyExistsAsync(request.IdempotencyKey, "HireEmployeeCommand", cancellationToken);
            if (IsRequestAlreadyProcessed) return request.Id;
            
            var fullName = new Fullname(request.FirstName, request.LastName, request.Title);
            var contactInfo = new ContactInfo(request.Street, request.City, request.PostalCode, request.PhoneNumber, request.EmailAddress);

            var newHire = Core.Domains.EmployeeAggregate.Employee.Hire(fullName, request.DepartmentId, contactInfo, request.InitialSalary, request.HireReason);

            foreach (var domainEvent in newHire.DomainEvents)
            {
               // await _publisher.Publish(domainEvent, ct);
            }

            newHire.ClearDomainEvents();
            return newHire.Id;
        }
    }
}
