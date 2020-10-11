namespace Owt.Services
{
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;

    using Owt.Data;
    using Owt.Services.Caching;

    public class DependenciesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component
                    .For<OwtDbContext>()
                    .LifestyleTransient(),
                Component
                    .For<ICache>()
                    .ImplementedBy<Cache>()
                    .DependsOn(Dependency.OnValue("cacheDurationInMinutes", 60))
                    .LifestyleSingleton()
            );
        }
    }
}