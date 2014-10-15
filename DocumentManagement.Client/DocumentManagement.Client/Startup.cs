using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DocumentManagement.Client.Startup))]
namespace DocumentManagement.Client
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
