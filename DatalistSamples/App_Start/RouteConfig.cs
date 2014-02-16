using System.Web.Mvc;
using System.Web.Routing;

namespace DatalistSamples
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Datalist",
                "{controller}/{action}",
                new { controller = "Datalist", action = "Home" },
                new { controller = "Datalist" }
            );

            routes.MapRoute(
                "Datalist/API",
                "Datalist/{controller}/{action}",
                new { controller = "API", action = "AbstractDatalist" }
            );

            routes.MapRoute(
                "Datalist/API/Classes",
                "Datalist/API/{controller}/{action}"
            );
        }
    }
}
