namespace Owt.Web
{
    using System;
    using System.IO;

    using Castle.Windsor;
    using Castle.Windsor.Installer;

    public static class IocContainer
    {
        internal static IWindsorContainer Container { get; private set; }

        public static void Setup(FileInfo castleConfigFile)
        {
            if (castleConfigFile == null)
            {
                throw new ArgumentNullException(nameof(castleConfigFile));
            }

            if (!castleConfigFile.Exists)
            {
                throw new FileNotFoundException("Unable to find file.", castleConfigFile.FullName);
            }

            Container = new WindsorContainer();

            Container.Install(Configuration.FromXmlFile(castleConfigFile.FullName));
        }

        public static void Dispose()
        {
            Container?.Dispose();
        }
    }
}