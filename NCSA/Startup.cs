using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NCSA.Startup))]
namespace NCSA
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
