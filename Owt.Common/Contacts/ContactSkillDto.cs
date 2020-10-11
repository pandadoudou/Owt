namespace Owt.Common.Contacts
{
    using Owt.Common.Lookups;

    public class ContactSkillDto
    {
        public int Id { get; set; }

        public int ContactId { get; set; }

        public LookupDto Skill { get; set; }

        public LookupDto ExpertiseLevel { get; set; }
    }
}