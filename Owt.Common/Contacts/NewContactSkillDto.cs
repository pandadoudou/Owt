namespace Owt.Common.Contacts
{
    using Owt.Common.Lookups;

    public class NewContactSkillDto
    {
        public int ContactId { get; set; }

        public LookupDto Skill { get; set; }

        public LookupDto ExpertiseLevel { get; set; }
    }
}