using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyGitTest123.Startup))]
namespace MyGitTest123
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
