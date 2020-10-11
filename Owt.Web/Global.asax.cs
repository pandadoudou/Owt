namespace Owt.Web
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Dispatcher;

    using AutoMapper;

    using Castle.MicroKernel.Registration;

    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            SetupCastle();
            ConfigureAutoMapperCastle();
            SetupControllers();
        }

        private static void SetupControllers()
        {
            IocContainer.Container.Register(
                Classes.FromThisAssembly()
                    .BasedOn<IHttpController>()
                    .LifestyleTransient());
        }

        private static void SetupCastle()
        {
            const string SettingsKey = "castleConfigFile";

            if (ConfigurationManager.AppSettings.AllKeys.All(k => !string.Equals(k, SettingsKey, StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new ApplicationException($"Unable to find settings key '{SettingsKey}'.");
            }

            IocContainer.Setup(new FileInfo(ConfigurationManager.AppSettings[SettingsKey]));

            GlobalConfiguration.Configuration.Services.Replace(
                typeof(IHttpControllerActivator),
                new WindsorCompositionRoot(IocContainer.Container));
        }

        private static void ConfigureAutoMapperCastle()
        {
            // Register all mapper profiles
            IocContainer.Container.Register(
                Classes.FromAssemblyInThisApplication(typeof(IocContainer).Assembly)
                    .BasedOn<Profile>()
                    .WithServiceBase());

            // Register IConfigurationProvider with all registered profiles
            IocContainer.Container.Register(
                Component.For<IConfigurationProvider>().UsingFactoryMethod(
                    kernel =>
                        {
                            return new MapperConfiguration(
                                configuration =>
                                    {
                                        kernel.ResolveAll<Profile>().ToList().ForEach(configuration.AddProfile);
                                    });
                        }).LifestyleSingleton());

            // Register IMapper with registered IConfigurationProvider
            IocContainer.Container.Register(
                Component.For<IMapper>().UsingFactoryMethod(
                    kernel =>
                        new Mapper(kernel.Resolve<IConfigurationProvider>(), kernel.Resolve)));

            // Register converters
            IocContainer.Container.Register(
                Classes.FromAssemblyInThisApplication(typeof(IocContainer).Assembly)
                    .BasedOn(typeof(ITypeConverter<,>))
                    .WithServiceSelf()
                    .LifestyleTransient());
        }

        public override void Dispose()
        {
            IocContainer.Dispose();
            base.Dispose();
        }
    }
}