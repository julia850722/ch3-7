using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ch3_7.Startup))]
namespace ch3_7
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
