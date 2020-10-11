namespace Owt.Data.Lookups
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class LookupRepository : ILookupRepository
    {
        private readonly OwtDbContext dbContext;

        public LookupRepository(OwtDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<List<TLookup>> GetLookupsAsync<TLookup>(CancellationToken cancellationToken)
            where TLookup : class, ILookup, new()
        {
            return await this.dbContext
                       .Set<TLookup>()
                       .Where(l => !l.Deleted)
                       .AsNoTracking()
                       .ToListAsync(cancellationToken);
        }
    }
}