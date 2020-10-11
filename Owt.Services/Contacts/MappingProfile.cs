namespace Owt.Services.Contacts
{
    using AutoMapper;

    using Owt.Common.Contacts;
    using Owt.Data.Contacts;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Contact, ContactDto>()
                .ForMember(dto => dto.FullName, opt => opt.MapFrom(src => $"{src.Firstname} {src.Lastname}".Trim()));
            this.CreateMap<ContactDto, Contact>();

            this.CreateMap<NewContactDto, Contact>();
            this.CreateMap<Contact, ContactExcerptDto>();
            this.CreateMap<Contact, ContactDetailsDto>();

            this.CreateMap<ContactSkill, ContactSkillDto>()
                .ConvertUsing<ContactSkillToDtoConverter>();

            this.CreateMap<NewContactSkillDto, ContactSkill>()
                .ConvertUsing<ContactSkillFromNewDtoConverter>();

            this.CreateMap<ContactSkillDto, ContactSkill>()
                .ConvertUsing<ContactSkillFromDtoConverter>();
        }
    }
}