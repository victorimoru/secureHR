using Azure.Messaging.ServiceBus;
using SecureHR.Core.DomainEvents;
using System.Text.Json;

namespace SecureHR.Application.MessageBroker
{
    public class AzureServiceBusMessageBus(string connectionString) : IMessageBus
    {
        private readonly ServiceBusClient _client = new(connectionString);

        public async Task PublishAsync(IDomainEvent @event, CancellationToken cancellationToken = default)
        {
            var topicName = @event.GetType().Name.ToLowerInvariant();

            await using var sender = _client.CreateSender(topicName);

            var messageBody = JsonSerializer.Serialize(@event, @event.GetType());

            var serviceBusMessage = new ServiceBusMessage(messageBody)
            {
                ContentType = "application/json",
                Subject = @event.GetType().Name,
            };

            await sender.SendMessageAsync(serviceBusMessage, cancellationToken);
        }
    }
}
