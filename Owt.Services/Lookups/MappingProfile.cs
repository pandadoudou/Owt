namespace Owt.Services.Lookups
{
    using AutoMapper;

    using Owt.Common.Lookups;
    using Owt.Data.Lookups;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Skill, LookupDto>();
            this.CreateMap<ExpertiseLevel, LookupDto>();
        }
    }
}