namespace Owt.Common.Contacts
{
    using System.Collections.Generic;

    public class ContactDetailsDto : IContactDto
    {
        public int Id { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string FullName { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string MobilePhoneNumber { get; set; }

        public List<ContactSkillDto> ContactSkills { get; set; }
    }
}