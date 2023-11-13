using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ThuchanhPTUDW.Startup))]
namespace ThuchanhPTUDW
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
