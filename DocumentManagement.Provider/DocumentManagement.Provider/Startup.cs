using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DocumentManagement.Provider.Startup))]
namespace DocumentManagement.Provider
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
