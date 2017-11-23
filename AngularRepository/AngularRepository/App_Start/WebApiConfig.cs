
using System.Web.Http;
using Newtonsoft.Json.Serialization;
namespace WebScraping
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            var jsonSettings = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            jsonSettings.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            config.Routes.MapHttpRoute(
                   name: "ControllerAndAction",
                   routeTemplate: "api/{controller}/{id}",
                   defaults: new { id = RouteParameter.Optional }
            );

          //config.Routes.MapHttpRoute(
          //      name: "ControllerAndAction",
          //      routeTemplate: "api/{controller}/{action}/{id}",
          //      defaults: new { id = RouteParameter.Optional }
          // );

          //config.Routes.MapHttpRoute(
          //        name: "ControllerAndAction2",
          //        routeTemplate: "api/{controller}/{action}/{title}",
          //        defaults: new { title = RouteParameter.Optional }
          //   );

          //config.Routes.MapHttpRoute(
          //    name: "ControllerAndId",
          //    routeTemplate: "api/{controller}/{action}"
          //);

        }
    }
}
