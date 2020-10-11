namespace Owt.Common.Contacts
{
    public interface IContactDto
    {
        int Id { get; set; }

        string Firstname { get; set; }

        string Lastname { get; set; }

        string FullName { get; set; }

        string Address { get; set; }

        string Email { get; set; }

        string MobilePhoneNumber { get; set; }
    }
}