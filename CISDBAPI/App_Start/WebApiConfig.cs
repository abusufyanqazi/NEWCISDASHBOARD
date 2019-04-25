using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;

namespace DashBoardAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes

            config.Routes.MapHttpRoute(
            name: "param2",
            routeTemplate: "{controller}/{code}/{fdr}",
            defaults: new { controller = "Values", action = "Get", code = RouteParameter.Optional, fdr = RouteParameter.Optional }
        );


            config.Routes.MapHttpRoute(
              name: "param3",
              routeTemplate: "{controller}/{code}/{batch}/{units}",
              defaults: new { controller = "Values", action = "Get", code = RouteParameter.Optional, batch = RouteParameter.Optional, units = RouteParameter.Optional}
          );


            config.Routes.MapHttpRoute(
                name: "param4",
                routeTemplate: "{controller}/{code}/{age}/{phase}/{trf}",
                defaults: new { controller = "Values", action = "Get", code = RouteParameter.Optional, age = RouteParameter.Optional, phase = RouteParameter.Optional, trf = RouteParameter.Optional}
            );

            config.Routes.MapHttpRoute(
                name: "param5",
                routeTemplate: "{controller}/{code}/{type}/{status}/{trf}/{slab}",
                defaults: new { controller = "Values", action = "Get", code = RouteParameter.Optional, type = RouteParameter.Optional, status = RouteParameter.Optional, trf = RouteParameter.Optional, slab = RouteParameter.Optional}
            );

            config.Routes.MapHttpRoute(
                name: "param7",
                routeTemplate: "{controller}/{rs}/{age}/{batch}/{pgvt}/{rundc}/{trf}/{srt}",
                defaults: new { controller = "Values", action = "Get", rs = RouteParameter.Optional, age = RouteParameter.Optional, batch = RouteParameter.Optional, pgvt = RouteParameter.Optional, rundc = RouteParameter.Optional, trf = RouteParameter.Optional, srt = RouteParameter.Optional}
            );

            config.Routes.MapHttpRoute(
                name: "Default",
                routeTemplate: "{controller}/{code}",
                defaults: new { controller = "Values", action = "Get", code = RouteParameter.Optional }
            );

            //config.Formatters.JsonFormatter.SupportedMediaTypes
            //    .Add(new MediaTypeHeaderValue("text/html"));
        }
    }
}
