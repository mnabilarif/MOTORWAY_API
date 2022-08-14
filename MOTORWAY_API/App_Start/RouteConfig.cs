using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MOTORWAY_API
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }


            );

            routes.MapRoute(
               name: "AddEntry",
               url: "{controller}/action/{id}",
               defaults: new { controller = "Motorway", action = "AddEntry", id = UrlParameter.Optional }

           );

            routes.MapRoute(
               name: "AddExit",
               url: "{controller}/action/{id}",
               defaults: new { controller = "Motorway", action = "AddExit", id = UrlParameter.Optional }

           );
            routes.MapRoute(
               name: "Test",
               url: "{controller}/action/{id}",
               defaults: new { controller = "Motorway", action = "Test", id = UrlParameter.Optional }

           );
        }
    }
}
