using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AxsisTest.Startup))]
namespace AxsisTest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
