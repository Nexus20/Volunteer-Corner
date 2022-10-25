using Microsoft.AspNetCore.Identity;
using Volunteer_Corner.Data.Entities.Identity;

namespace Volunteer_Corner.Business.Infrastructure;

public class IdentityInitializer : IIdentityInitializer
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;

    public IdentityInitializer(UserManager<User> userManager, RoleManager<Role> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public void InitializeIdentityData()
    {
        InitializeSuperAdminRole().Wait();
        RegisterRoleAsync(CustomRoles.AdminRole).Wait();
        RegisterRoleAsync(CustomRoles.ModeratorRole).Wait();
        RegisterRoleAsync(CustomRoles.UserRole).Wait();
        RegisterRoleAsync(CustomRoles.VolunteerRole).Wait();
        RegisterRoleAsync(CustomRoles.HelpSeekerRole).Wait();
    }
    
    private async Task<Role> RegisterRoleAsync(string roleName)
    {

        var role  = await _roleManager.FindByNameAsync(roleName);

        if (role != null) {
            return role;
        }

        role = new Role(roleName);
        await _roleManager.CreateAsync(role);

        return role;
    }
    
    private async Task InitializeSuperAdminRole() {

        var superAdmin = _userManager.Users.FirstOrDefault(u => u.UserName == "root") ?? RegisterSuperAdmin();
            
        var superAdminRole = await RegisterRoleAsync(CustomRoles.SuperAdminRole);

        if(!await _userManager.IsInRoleAsync(superAdmin, CustomRoles.SuperAdminRole))
            await _userManager.AddToRoleAsync(superAdmin, superAdminRole.Name);
    }
    
    private User RegisterSuperAdmin() {

        var superAdmin = new User() {
            FirstName = "Root",
            LastName = "Root",
            UserName = "root",
            Patronymic = "Root",
            Email = "root@volunteer-corner.com"
        };

        _userManager.CreateAsync(superAdmin, "_QGrXyvcmTD4aVQJ_").Wait();

        return superAdmin;
    }
}