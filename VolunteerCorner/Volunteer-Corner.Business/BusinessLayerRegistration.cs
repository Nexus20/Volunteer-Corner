using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volunteer_Corner.Data;

namespace Volunteer_Corner.Business;

public static class BusinessLayerRegistration
{
    public static IServiceCollection AddBusinessLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDataLayer(configuration);

        return services;
    }
}