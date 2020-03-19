using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PetGrooming.Startup))]
namespace PetGrooming
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
