namespace Owt.Services.Contacts
{
    using System.Collections.Generic;

    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;

    using Owt.Data.Contacts;

    public class Installer : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component
                    .For<IContactService>()
                    .ImplementedBy<ContactService>()
                    .LifestyleTransient(),
                Component
                    .For<IContactRepository>()
                    .ImplementedBy<ContactRepository>()
                    .LifestyleTransient(),
                Component
                    .For<IContactValidator>()
                    .ImplementedBy<ContactValidator>()
                    .DependsOn(
                        Dependency.OnValue(
                            "supportedValidationRegions",
                            new HashSet<string>(new[] { "CH", "FR" })))
                    .LifestyleTransient()
            );
        }
    }
}