using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Extensions;
using Microsoft.Owin.StaticFiles;
using Owin;

[assembly: OwinStartup(typeof(TimeTracker.Web.Startup))]

namespace TimeTracker.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            app.UseWebApi(config);
            app.UseStaticFiles();
            app.UseStageMarker(PipelineStage.MapHandler);

        }
    }
}
