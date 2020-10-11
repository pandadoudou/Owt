namespace Owt.Services.Contacts
{
    using System;

    using AutoMapper;

    using Owt.Common.Contacts;
    using Owt.Common.Lookups;
    using Owt.Data.Contacts;
    using Owt.Services.Lookups;

    public class ContactSkillToDtoConverter : ITypeConverter<ContactSkill, ContactSkillDto>
    {
        private readonly ILookupService lookupService;

        public ContactSkillToDtoConverter(ILookupService lookupService)
        {
            this.lookupService = lookupService ?? throw new ArgumentNullException(nameof(lookupService));
        }

        public ContactSkillDto Convert(ContactSkill source, ContactSkillDto destination, ResolutionContext context)
        {
            if (destination == null)
            {
                destination = new ContactSkillDto();
            }

            if (source == null)
            {
                return destination;
            }

            destination.Id = source.Id;

            LookupDto skill = AsyncHelper.RunSync(() => this.lookupService.GetSkillAsync(source.SkillId, default));
            destination.Skill = context.Mapper.Map<LookupDto>(skill);

            LookupDto expertiseLevel = AsyncHelper.RunSync(() => this.lookupService.GetExpertiseLevelAsync(source.ExpertiseLevelId, default));
            destination.ExpertiseLevel = context.Mapper.Map<LookupDto>(expertiseLevel);
            destination.ContactId = source.ContactId;

            return destination;
        }
    }
}