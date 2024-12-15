using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(QuanLyTaiSan_UserManagement.Startup))]
namespace QuanLyTaiSan_UserManagement
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
