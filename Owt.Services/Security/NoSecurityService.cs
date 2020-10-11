namespace Owt.Services.Security
{
    using Owt.Data;

    public class NoSecurityService : ISecurityService
    {
        public void Ensure(SecurityEntityTypes contact, SecurityMethods create)
        {
        }

        public string GetCurrentIdentityName() => "NoSecurity";

        public void EnsureOwnership(IOwnable ownable)
        {
        }
    }
}