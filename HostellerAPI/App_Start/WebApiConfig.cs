
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;

namespace HostellerAPI
{
    public static class WebApiConfig
    {
        public static bool IsTokenRequired = false;

        public static void Register(HttpConfiguration config)
        {

            //KI.RIS.Common.Business.BLGlobalCaching objBLGlobalCaching = new BLGlobalCaching();
            //objBLGlobalCaching.SetGlobalData();

            ///TokenChecking();

            // Web API configuration and services

            /// Configure web api to use only bearer token authentication. //Install-Package Microsoft.AspNet.WebApi.Owin -Version 5.2.4

            //config.SuppressDefaultHostAuthentication();
            // config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));



            //  config.SetCorsPolicyProviderFactory(new CorsPolicyFactory());

            ///////////////////To enable json in browser..bydefault its xml
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("text/html"));


            //config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));


            var JSON = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            JSON.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;

            /////////////////////////////////////////To caste the Pascalcase (.net) to Camelcasing(json format)  
            ///////////////////////////////////////---it will convert pascal casing to camelcasing globally.

            ///config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();


            /////////////////////////////////////To configure the json format : Version,status code, Result ...
            //
            config.MessageHandlers.Add(new WrappingHandler());

            config.Filters.Add(new APIExceptionFilterAttribute());

            //////////////////////////////// To enable the Authorization (bearer token)
            if (!IsTokenRequired)
            {
                //  config.Filters.Add(new AuthorizeAttribute());
            }
            else
            {
                //////////////////////////////// To enable the CORS 
                //var cors = new EnableCorsAttribute("*", "*", "*");// { SupportsCredentials=true};
                //config.EnableCors(cors); /// Not required in token calling
            }

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        private static void TokenChecking()
        {
            //  var Token = GlobalCache.GlobalCaching.GenApplicationSetting.AsEnumerable().Where(r =>
            //Convert.ToString(r["Setting"]).Equals("TokenAuthenticationRequired"));
            //  if (Convert.ToString(Token.ElementAt(0)["Value"]) == "1")
            //  {
            //      IsTokenRequired = true;
            //  }
        }
    }
}
