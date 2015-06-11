using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FileCompressor.Startup))]
namespace FileCompressor
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
