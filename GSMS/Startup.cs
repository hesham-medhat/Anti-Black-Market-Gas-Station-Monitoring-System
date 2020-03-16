using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GSMS.Startup))]
namespace GSMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
