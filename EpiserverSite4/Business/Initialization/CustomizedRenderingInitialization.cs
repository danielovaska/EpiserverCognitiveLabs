using System.Web.Mvc;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EpiserverSite4.Business.Rendering;
using EPiServer.Web;
using EPiServer.Approvals.ContentApprovals;
using EPiServer.Core;
using EPiServer.Approvals;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;

namespace EpiserverSite4.Business.Initialization
{
    /// <summary>
    /// Module for customizing templates and rendering.
    /// </summary>
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class CustomizedRenderingInitialization : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            //Add custom view engine allowing partials to be placed in additional locations
            //Note that we add it first in the list to optimize view resolving when using DisplayFor/PropertyFor
            ViewEngines.Engines.Insert(0, new SiteViewEngine());

            context.Locate.TemplateResolver()
                .TemplateResolved += TemplateCoordinator.OnTemplateResolved;
           
        }
     

        public void Uninitialize(InitializationEngine context)
        {
            ServiceLocator.Current.GetInstance<TemplateResolver>()
                .TemplateResolved -= TemplateCoordinator.OnTemplateResolved;
        }

        public void Preload(string[] parameters)
        {
        }
    }
}
