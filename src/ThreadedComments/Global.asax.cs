using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ThreadedComments.Data.Entities;

namespace ThreadedComments
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        #region Constants

        internal const string CurrentRequestThreadedCommentsContext = "CurrentRequestLeaderboardContext";

        #endregion


        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Items[CurrentRequestThreadedCommentsContext] = new ThreadedCommentsEntities();
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            using (var ctx = (ThreadedCommentsEntities)HttpContext.Current.Items[CurrentRequestThreadedCommentsContext])
            {
                if (ctx == null)
                {
                    // No context created; nothing to do.
                    return;
                }

                if (Server.GetLastError() != null)
                {
                    // There was an unhandled exception. Don't save changes to the context.
                    return;
                }

                ctx.SaveChanges();
            }

            //TODO: Do I need background tasks?
            //TaskExecutor.StartExecuting();
        }
    }
}