using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volunteer_Corner.Business.Infrastructure;
using Volunteer_Corner.Business.Interfaces;
using Volunteer_Corner.Business.Services;
using Volunteer_Corner.Data;

namespace Volunteer_Corner.Business;

public static class BusinessLayerRegistration
{
    public static IServiceCollection AddBusinessLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDataLayer(configuration);
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddScoped<IIdentityInitializer, IdentityInitializer>();

        services.AddScoped<IUserService, UserService>();

        return services;
    }
}