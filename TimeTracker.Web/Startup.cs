using Microsoft.Owin;
using Microsoft.Owin.Extensions;
using Owin;
using System.Web.Http;
using TimeTracker.Web;

[assembly: OwinStartup(typeof(Startup))]

namespace TimeTracker.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            SimpleLogger.ConfigureLogger(app);
            ConfigureAuth(app);
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            app.UseWebApi(config);
            app.UseStageMarker(PipelineStage.MapHandler);
        }
    }
}
