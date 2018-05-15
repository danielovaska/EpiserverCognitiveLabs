using System.Web.Mvc;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EpiserverSite4.Business.Rendering;
using EPiServer.Web.Mvc;
using EPiServer.Web.Mvc.Html;
using EpiserverSite4.Controllers;
using Mogul.Interceptor.Base.Infrastructure.IoC;
using Mogul.Interceptor.Logging;
using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using Mogul.Interceptor.Cache;
using EPiServer.Logging;
using Mogul.Interceptor.Base.Infrastructure.Logging;

namespace EpiserverSite4.Business.Initialization
{
    [InitializableModule]
    public class DependencyResolverInitialization : IConfigurableModule
    {
        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            //Implementations for custom interfaces can be registered here.
            //var textAnalyticsLogger = LogManager.GetLogger(typeof(TextAnalyticsRepository));
            context.ConfigurationComplete += (o, e) =>
            {

                //Register custom implementations that should be used in favour of the default implementations
                context.Services
                .AddTransient<IContentRenderer, ErrorHandlingContentRenderer>()
                .AddTransient<ContentAreaRenderer, AlloyContentAreaRenderer>()
                .AddTransient<INewsRepository, NewsRepository>();
               
            };
        }
        public void Initialize(InitializationEngine context)
        {
            DependencyResolver.SetResolver(new ServiceLocatorDependencyResolver(context.Locate.Advanced));
        }

        public void Uninitialize(InitializationEngine context)
        {
        }

        public void Preload(string[] parameters)
        {
        }
    }
}
