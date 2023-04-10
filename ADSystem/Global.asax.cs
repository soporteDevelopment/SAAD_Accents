using ADSystem.App_Start;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ADSystem
{
	public class MvcApplication : System.Web.HttpApplication
    {
        public object WebApiConfig { get; private set; }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            // Manually installed WebAPI 2.2 after making an MVC project.
            GlobalConfiguration.Configure(WebApiApplication.Register); 
            // NEW way
            //WebApiConfig.Register(GlobalConfiguration.Configuration); // DEPRECATED

            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            IoCConfiguration.Configure();
        }
    }
}
