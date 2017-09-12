using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SiteProjet.Startup))]
namespace SiteProjet
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
