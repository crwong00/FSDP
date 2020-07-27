using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FSDP.UI.MVC.Startup))]
namespace FSDP.UI.MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
