namespace Owt.Common.Contacts
{
    public interface IValidableContactDto
    {
        string Firstname { get; }

        string Lastname { get; }

        string Address { get; }

        string Email { get; }

        string MobilePhoneNumber { get; }
    }
}