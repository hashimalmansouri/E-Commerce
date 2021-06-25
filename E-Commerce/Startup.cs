using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using E_Commerce.Models;

[assembly: OwinStartupAttribute(typeof(E_Commerce.Startup))]
namespace E_Commerce
{
    public partial class Startup
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateDefaultRolesAndUsers();
        }
        public void CreateDefaultRolesAndUsers()
        {
            var roleManager = new RoleManager<IdentityRole>(new
                RoleStore<IdentityRole>(db));
            var userManager = new UserManager<ApplicationUser>(new
                UserStore<ApplicationUser>(db));
            IdentityRole role = new IdentityRole();
            if (!roleManager.RoleExists("Admin"))
            {
                role.Name = "Admin";
                roleManager.Create(role);
                ApplicationUser user = new ApplicationUser();
                user.FirstName = "HEAR";
                user.LastName = "TEAM";
                user.Email = "HEAR@gmail.com";
                user.UserName = "HEARTeam";
                var Check = userManager.Create(user, "Hear@123");
                if (Check.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Admin");
                }
            }
        }
    }
}