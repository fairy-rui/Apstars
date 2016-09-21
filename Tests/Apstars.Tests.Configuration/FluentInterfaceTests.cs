using Microsoft.VisualStudio.TestTools.UnitTesting;
using Apstars.Bootstrapper;
using Apstars.Config.Fluent;
using Apstars.ObjectContainers.Autofac;
using Autofac;

namespace Apstars.Tests.Configuration
{
    [TestClass]
    public class FluentInterfaceTests
    {
        [TestInitialize()]
        public void Configuration_Initialize()
        {
            var application = AppRuntime.Instance.ConfigureApstars().WithDefaultSettings().UsingAutofacContainer().Create();
            application.ObjectContainer.RegisterAssemblyModules(typeof(TestService).Assembly).Build();

            application.Start();
        }

        [TestMethod]
        public void Configuration_CreateAppWithDefaultSettingsTest()
        {
            //var application = AppRuntime.Instance.ConfigureApstars().WithDefaultSettings().UsingAutofacContainer().Create();
            //application.ObjectContainer.RegisterAssemblyModules(typeof(TestService).Assembly).Build();

            //application.Start();
            //Assert.IsNotNull(application);
            
            var service = ServiceLocator.Instance.GetService<IService>();
            Assert.IsNotNull(service);
            Assert.IsInstanceOfType(service, typeof(IService));
        }
    }

    public interface IService { }
    public class TestService : IService { }

    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TestService>().As<IService>();
        }
    }
}
