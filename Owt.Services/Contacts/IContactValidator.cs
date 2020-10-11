namespace Owt.Services.Contacts
{
    using Owt.Common.Contacts;

    public interface IContactValidator
    {
        void ValidateContact(IValidableContactDto validableContactDto);
    }
}