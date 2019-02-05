using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using Vidly.Models;

[assembly: OwinStartupAttribute(typeof(Vidly.Startup))]
namespace Vidly
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            
		}
    }
}
