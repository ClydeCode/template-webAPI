using template_webApi.Models;
using Microsoft.AspNetCore.Identity;

namespace template_webApi.Data
{
    public class ApplicationDbContextSeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, IConfiguration config)
        {
            var services = serviceProvider;
            var roleManager = serviceProvider.GetService<RoleManager<ApplicationRole>>();
            var roles = config.GetSection("AppSettings").GetSection("Roles").Get<List<string>>();

            if (roles != null)
            {
                foreach (var role in roles)
                {
                    await EnsureRole(roleManager, role);
                }
            }
  
            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
            var users = config.GetSection("AppSettings").GetSection("AdminUsers").Get<List<AdminUserModel>>();
            var password = config.GetSection("AppSettings").GetSection("Password").Get<string>();

            if (users != null)
            {
                foreach (var user in users)
                {
                    await EnsureUser(userManager, user, password);
                }
            }
        }

        private static async Task<Guid> EnsureRole(RoleManager<ApplicationRole> roleManager, string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);

            if (role == null)
            {
                role = new ApplicationRole { Name = roleName };

                await roleManager.CreateAsync(role);
            }

            return role.Id;
        }

        private static async Task<Guid> EnsureUser(UserManager<ApplicationUser> userManager, AdminUserModel userModel, string password)
        {
            try
            {
                var user = await userManager.FindByNameAsync(userModel.UserName);

                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = userModel.UserName,
                        Email = userModel.UserName,
                        FirstName = userModel.FirstName,
                        LastName = userModel.LastName,
                        EmailConfirmed = true
                    };

                    var hasher = new PasswordHasher<ApplicationUser>();
                    var hashed = hasher.HashPassword(user, password);

                    user.PasswordHash = hashed;

                    var result = await userManager.CreateAsync(user);

                    result = await userManager.AddToRoleAsync(user, "Administrator");
                }

                return user.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
