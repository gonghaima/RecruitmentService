using MvcApplication1.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcApplication1
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        public class OptionalAuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
        {
            private readonly bool _authorize;

            public OptionalAuthorizeAttribute()
            {
                _authorize = true;
            }

            public OptionalAuthorizeAttribute(bool authorize)
            {
                _authorize = authorize;
            }

            protected override bool AuthorizeCore(HttpContextBase httpContext)
            {
                if (!_authorize)
                    return true;

                //string[] roles = rolesProvider.GetRolesForUser(httpContext.User.Identity.Name);
                dataEntities dbContext = new dataEntities();
                var roles = dbContext.Users.FirstOrDefault(m => m.UserName == httpContext.User.Identity.Name).Roles;

                bool boRo=false;
                List<string> RoleNames = new List<string>();
                foreach (Role r in roles)
                {
                    RoleNames.Add(r.Name);
                }

                foreach (string strRole in RoleNames)
                {
                    if (Roles.Contains(strRole))
                        boRo = true;
                }

                //if(RoleNames.Contains(Website.Roles.RegisteredClient, StringComparer.OrdinalIgnoreCase)

                //return base.AuthorizeCore(httpContext) && boRo;
                return boRo;
                //return base.AuthorizeCore(httpContext);
            }

            protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
            {
                
                if (filterContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    //base.HandleUnauthorizedRequest(filterContext);
                    filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));
                }
            }
        }
    }
}