namespace Owt.Services.Security
{
    using System;
    using System.Collections.Generic;
    using System.Security;
    using System.Threading;

    using Owt.Data;

    public class DummySecurityService : ISecurityService
    {
        private readonly Dictionary<string, HashSet<string>> dummyUserRights = new Dictionary<string, HashSet<string>>(StringComparer.InvariantCultureIgnoreCase);

        public DummySecurityService()
        {
            // TODO : retrieve from Repository+Database
            this.dummyUserRights.Add(
                @"W10X64-DEV\Proxmox",
                new HashSet<string>(StringComparer.InvariantCultureIgnoreCase)
                    {
                        GetDummyRightName(SecurityEntityTypes.Contact, SecurityMethods.View),
                        GetDummyRightName(SecurityEntityTypes.Contact, SecurityMethods.Create),
                        GetDummyRightName(SecurityEntityTypes.Contact, SecurityMethods.Update),
                        GetDummyRightName(SecurityEntityTypes.Contact, SecurityMethods.Delete),

                        GetDummyRightName(SecurityEntityTypes.ContactSkills, SecurityMethods.View),
                        GetDummyRightName(SecurityEntityTypes.ContactSkills, SecurityMethods.Create),
                        GetDummyRightName(SecurityEntityTypes.ContactSkills, SecurityMethods.Update),
                        GetDummyRightName(SecurityEntityTypes.ContactSkills, SecurityMethods.Delete)
                    });
        }

        public void Ensure(SecurityEntityTypes entityType, SecurityMethods method)
        {
            string rightToCheck = GetDummyRightName(entityType, method);
            string currentIdentityName = this.GetCurrentIdentityName();

            if (!this.dummyUserRights.ContainsKey(currentIdentityName) ||
                !this.dummyUserRights[currentIdentityName].Contains(rightToCheck))
            {
                throw new SecurityException();
            }
        }

        public string GetCurrentIdentityName()
        {
            string currentIdentityName = Thread.CurrentPrincipal?.Identity?.Name;
            if (string.IsNullOrWhiteSpace(currentIdentityName))
            {
                throw new ApplicationException($"Unable to retrieve the current identity.");
            }

            return currentIdentityName;
        }

        public void EnsureOwnership(IOwnable ownable)
        {
            if (ownable == null)
            {
                throw new ArgumentNullException(nameof(ownable));
            }

            if (!string.Equals(ownable.CreatedBy, GetCurrentIdentityName(), StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityException();
            }
        }

        private static string GetDummyRightName(SecurityEntityTypes entityType, SecurityMethods method) => $"{Enum.GetName(entityType.GetType(), entityType)}_{Enum.GetName(method.GetType(), method)}";
    }
}