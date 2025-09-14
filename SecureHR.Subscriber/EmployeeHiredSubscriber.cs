using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SecureHR.Core.DomainEvents;
using SecureHR.Core.Domains.EmployeeAggregate;
using SecureHR.Core.Repositories;
using SecureHR.Infrastructure.Repositories;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SecureHR.Subscriber
{
    public class EmployeeHiredSubscriber(
        IUnitOfWork unitOfWork,
        IEmployeeRepository employeeRepository,
        IIdempotencyRepository idempotencyRepository,
        ILogger<EmployeeHiredSubscriber> logger)
    {
        [FunctionName("ProcessHiredEmployee")]
        public async Task Run(
            [ServiceBusTrigger(topicName: "employeehiredevent", subscriptionName: "security", Connection = "AzureServiceBus")]
            string messageBody,
            CancellationToken cancellationToken)
        {
            logger.LogInformation("--- Security Department: New EmployeeHiredEvent received! ---");

            try
            {
                var hiredEvent = JsonSerializer.Deserialize<EmployeeHiredEvent>(messageBody);
                if (hiredEvent is null)
                {
                    logger.LogWarning("Failed to deserialize message body. Message will be discarded.");
                    return;
                }

                var isRequestAlreadyProcessed = await idempotencyRepository.KeyExistsAsync(hiredEvent.IdempotencyKey, "ProcessedHiredEmployee", cancellationToken);

                if (isRequestAlreadyProcessed)
                {
                    logger.LogWarning("Event with idempotency key {Key} has already been processed by this subscriber. Skipping.", hiredEvent.IdempotencyKey);
                    return; 
                }

                logger.LogInformation("ACTION: An access card should now be provisioned for Employee ID: {EmployeeId}", hiredEvent.Id);

                var newHire = Employee.Hire(hiredEvent.Name, hiredEvent.DepartmentId, hiredEvent.Contact, hiredEvent.InitialSalary, hiredEvent.HireReason);
                await employeeRepository.AddAsync(newHire, cancellationToken);
                await idempotencyRepository.CreateKeyAsync(hiredEvent.IdempotencyKey, "ProcessedHiredEmployee", cancellationToken);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                logger.LogInformation("Successfully processed and recorded idempotency key for employee {EmployeeId}", hiredEvent.Id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while processing the EmployeeHiredEvent.");
                throw;
            }
        }
    }
}
