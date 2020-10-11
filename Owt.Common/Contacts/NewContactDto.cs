namespace Owt.Common.Contacts
{
    public class NewContactDto : IValidableContactDto
    {
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string MobilePhoneNumber { get; set; }
    }
}