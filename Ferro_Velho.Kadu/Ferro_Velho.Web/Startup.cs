using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Ferro_Velho.Web.Startup))]
namespace Ferro_Velho.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
