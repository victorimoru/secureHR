using Microsoft.EntityFrameworkCore;
using SecureHR.Core.Domains.Idempotency;
using SecureHR.Core.Repositories;
using SecureHR.Infrastructure.Data;

namespace SecureHR.Infrastructure.Repositories
{
    public class SqlIdempotencyRepository(ApplicationDbContext context) : IIdempotencyRepository
    {
        public async Task<bool> KeyExistsAsync(Guid key, string requestName, CancellationToken cancellationToken)
        {
            return await context.IdempotencyKeys
                .AnyAsync(ik => ik.Id == key && ik.RequestName == requestName, cancellationToken);
        }

        public async Task CreateKeyAsync(Guid key, string requestName, CancellationToken cancellationToken)
        {
            var idempotencyKey = new IdempotencyKey(key, requestName);
            await context.IdempotencyKeys.AddAsync(idempotencyKey, cancellationToken);
        }
    }
}
