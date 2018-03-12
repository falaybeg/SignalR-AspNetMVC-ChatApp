using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SignalR_SampleProject.Startup))]
namespace SignalR_SampleProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
