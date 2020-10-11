namespace Owt.Data.Lookups
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public interface ILookupRepository
    {
        Task<List<TLookup>> GetLookupsAsync<TLookup>(CancellationToken cancellationToken)
            where TLookup : class, ILookup, new();
    }
}