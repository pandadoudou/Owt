namespace Owt.Services.Lookups
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using AutoMapper;

    using Owt.Common.Lookups;
    using Owt.Data;
    using Owt.Data.Lookups;
    using Owt.Services.Caching;

    public class LookupService : ILookupService
    {
        private readonly IMapper mapper;
        private readonly ILookupRepository lookupRepository;
        private readonly ICache cache;

        public LookupService(
            IMapper mapper,
            ILookupRepository lookupRepository,
            ICache cache)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.lookupRepository = lookupRepository ?? throw new ArgumentNullException(nameof(lookupRepository));
            this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<List<LookupDto>> GetSkillsAsync(CancellationToken cancellationToken)
        {
            return await this.GetLookupDtosAsync<Skill>(cancellationToken);
        }

        public async Task<LookupDto> GetSkillAsync(int id, CancellationToken cancellationToken)
        {
            return await this.GetLookupDtoAsync<Skill>(id, cancellationToken);
        }

        public async Task<List<LookupDto>> GetExpertiseLevelsAsync(CancellationToken cancellationToken)
        {
            return await this.GetLookupDtosAsync<ExpertiseLevel>(cancellationToken);
        }

        public async Task<LookupDto> GetExpertiseLevelAsync(int id, CancellationToken cancellationToken)
        {
            return await this.GetLookupDtoAsync<ExpertiseLevel>(id, cancellationToken);
        }

        private async Task<LookupDto> GetLookupDtoAsync<TLookup>(int id, CancellationToken cancellationToken)
            where TLookup : class, ILookup, new()
        {
            LookupDto lookupDto = (await this.GetLookupDtosAsync<TLookup>(cancellationToken)).FirstOrDefault(l => l.Id == id);

            if (lookupDto == null)
            {
                throw new ArgumentException($"There is no {typeof(TLookup).Name} with Id {id}.");
            }

            return lookupDto;
        }

        private async Task<List<LookupDto>> GetLookupDtosAsync<TLookup>(CancellationToken cancellationToken)
            where TLookup : class, ILookup, new()
        {
            List<TLookup> cachedItems = this.cache.Get<TLookup>();

            if (cachedItems == null)
            {
                cachedItems = await this.lookupRepository.GetLookupsAsync<TLookup>(cancellationToken);
                this.cache.Set(cachedItems);
            }

            List<LookupDto> lookupDtos = cachedItems
                .Where(l => !l.Deleted)
                .Select(this.mapper.Map<LookupDto>).ToList();

            return lookupDtos;
        }
    }
}