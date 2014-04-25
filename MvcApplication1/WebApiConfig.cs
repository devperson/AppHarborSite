using System;
using System.Web;
using System.Web.Http;
using System.Web.Http.WebHost;
using System.Web.Routing;
using System.Web.SessionState;

namespace MvcApplication1
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //
            var route = config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
                            
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;            
            config.Formatters.Remove(config.Formatters.XmlFormatter);                        
        }       
    }
}