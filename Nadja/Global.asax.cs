using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Nadja
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static Bot Bot;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Bot = new Bot();
        }
    }
}
