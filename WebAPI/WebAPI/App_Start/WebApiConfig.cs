using System.Web.Http;
using Newtonsoft.Json.Serialization;

namespace WebAPI
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

    }
  }
}
