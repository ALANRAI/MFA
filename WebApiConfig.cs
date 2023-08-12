using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Web.Http;

namespace MFA_assignment
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {           
            // Web API configuration and services
            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Filters.Add(new AuthorizeAttribute());
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}


