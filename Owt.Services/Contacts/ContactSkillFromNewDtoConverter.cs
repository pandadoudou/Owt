namespace Owt.Services.Contacts
{
    using System;

    using AutoMapper;

    using Owt.Common.Contacts;
    using Owt.Data.Contacts;
    using Owt.Services.Lookups;

    public class ContactSkillFromNewDtoConverter : ITypeConverter<NewContactSkillDto, ContactSkill>
    {
        private readonly ILookupService lookupService;

        public ContactSkillFromNewDtoConverter(ILookupService lookupService)
        {
            this.lookupService = lookupService ?? throw new ArgumentNullException(nameof(lookupService));
        }

        public ContactSkill Convert(NewContactSkillDto source, ContactSkill destination, ResolutionContext context)
        {
            if (destination == null)
            {
                destination = new ContactSkill();
            }

            if (source == null)
            {
                return destination;
            }

            if (source.Skill == null)
            {
                throw new ArgumentException($"{nameof(source.Skill)} must be specified.");
            }

            destination.SkillId = AsyncHelper.RunSync(() => this.lookupService.GetSkillAsync(source.Skill.Id, default)).Id;

            if (source.ExpertiseLevel == null)
            {
                throw new ArgumentException($"{nameof(source.ExpertiseLevel)} must be specified.");
            }

            destination.ExpertiseLevelId = AsyncHelper.RunSync(() => this.lookupService.GetExpertiseLevelAsync(source.ExpertiseLevel.Id, default)).Id;

            destination.ContactId = source.ContactId;

            return destination;
        }
    }
}