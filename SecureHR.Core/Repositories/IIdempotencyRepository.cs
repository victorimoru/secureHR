namespace SecureHR.Core.Repositories
{
    public interface IIdempotencyRepository
    {

        Task<bool> KeyExistsAsync(Guid key, string requestName, CancellationToken cancellationToken);
        Task CreateKeyAsync(Guid key, string requestName, CancellationToken cancellationToken);
    }
}
