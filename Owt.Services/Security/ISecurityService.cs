namespace Owt.Services.Security
{
    using Owt.Data;

    public interface ISecurityService
    {
        void Ensure(SecurityEntityTypes contact, SecurityMethods create);

        string GetCurrentIdentityName();

        void EnsureOwnership(IOwnable ownable);
    }
}