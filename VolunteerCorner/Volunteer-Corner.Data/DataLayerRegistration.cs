using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volunteer_Corner.Data.Entities.Identity;
using Volunteer_Corner.Data.Interfaces;
using Volunteer_Corner.Data.Repositories;

namespace Volunteer_Corner.Data;

public static class DataLayerRegistration
{
    public static IServiceCollection AddDataLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        
        services.AddIdentity<User, Role>()
            .AddUserStore<UserStore<User, Role, ApplicationDbContext, string, IdentityUserClaim<string>, UserRole,
                IdentityUserLogin<string>, IdentityUserToken<string>, IdentityRoleClaim<string>>>()
            .AddRoleStore<RoleStore<Role, ApplicationDbContext, string, UserRole, IdentityRoleClaim<string>>>()
            .AddSignInManager<SignInManager<User>>()
            .AddRoleManager<RoleManager<Role>>()
            .AddUserManager<UserManager<User>>();

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        return services;
    }
}