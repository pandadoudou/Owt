namespace Owt.Services.Lookups
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;

    using Owt.Data.Lookups;

    public class Installer : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component
                    .For<ILookupService>()
                    .ImplementedBy<LookupService>()
                    .LifestyleTransient(),
                Component
                    .For<ILookupRepository>()
                    .ImplementedBy<LookupRepository>()
                    .LifestyleTransient()
            );
        }
    }
}