namespace SecureHR.Core.Domains.Idempotency
{
    public class IdempotencyKey(Guid id, string requestName)
    {
        public Guid Id { get; private set; } = id;
        public string RequestName { get; private set; } = requestName;
        public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;
    }
}
