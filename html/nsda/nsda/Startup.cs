using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(nsda.Startup))]
namespace nsda
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
