using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SignalR_ChatProject.Startup))]
namespace SignalR_ChatProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
