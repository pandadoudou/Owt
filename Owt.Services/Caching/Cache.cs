namespace Owt.Services.Caching
{
    using System;
    using System.Collections.Generic;

    public class Cache : ICache
    {
        private DateTime cacheInitializationDate;
        private readonly int cacheDurationInMinutes;
        private readonly Dictionary<string, object> dico = new Dictionary<string, object>();

        public Cache(int cacheDurationInMinutes)
        {
            this.cacheDurationInMinutes = cacheDurationInMinutes;
            this.Invalidate();
        }

        public void Set<TEntity>(List<TEntity> entities)
        {
            string cacheKey = GetCacheKey<TEntity>();

            if (!this.dico.ContainsKey(cacheKey))
            {
                this.dico.Add(cacheKey, null);
            }

            this.dico[cacheKey] = entities;
        }

        public List<TEntity> Get<TEntity>()
        {
            if (this.MustInvalidate())
            {
                this.Invalidate();
            }

            string cacheKey = GetCacheKey<TEntity>();

            if (this.dico.ContainsKey(cacheKey))
            {
                return (List<TEntity>)this.dico[cacheKey];
            }

            return null;
        }

        private bool MustInvalidate() => (DateTime.UtcNow - this.cacheInitializationDate).TotalMinutes > this.cacheDurationInMinutes;

        public void Invalidate()
        {
            this.cacheInitializationDate = DateTime.UtcNow;
            this.dico.Clear();
        }

        private static string GetCacheKey<TEntity>() => typeof(TEntity).FullName;
    }
}