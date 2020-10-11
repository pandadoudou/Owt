namespace Owt.Services.Lookups
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using Owt.Common.Lookups;

    public interface ILookupService
    {
        Task<List<LookupDto>> GetSkillsAsync(CancellationToken cancellationToken);

        Task<LookupDto> GetSkillAsync(int id, CancellationToken cancellationToken);

        Task<List<LookupDto>> GetExpertiseLevelsAsync(CancellationToken cancellationToken);

        Task<LookupDto> GetExpertiseLevelAsync(int id, CancellationToken cancellationToken);
    }
}