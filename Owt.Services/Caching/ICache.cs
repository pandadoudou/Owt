namespace Owt.Services.Caching
{
    using System.Collections.Generic;

    public interface ICache
    {
        void Set<TEntity>(List<TEntity> entities);

        List<TEntity> Get<TEntity>();

        void Invalidate();
    }
}