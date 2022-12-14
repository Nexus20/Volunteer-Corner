using System.Reflection;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volunteer_Corner.Business.Infrastructure;
using Volunteer_Corner.Business.Infrastructure.FileStorage;
using Volunteer_Corner.Business.Interfaces.Infrastructure;
using Volunteer_Corner.Business.Interfaces.Services;
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
        services.AddScoped<ISignInService, SignInService>();
        services.AddScoped<IHelpRequestService, HelpRequestService>();
        services.AddScoped<IHelpSeekerService, HelpSeekerService>();
        services.AddScoped<JwtHandler>();
        
        var blobStorageConnectionString = configuration.GetValue<string>("BlobStorageSettings:ConnectionString");
        services.AddSingleton(x => new BlobServiceClient(blobStorageConnectionString));
        services.AddScoped<IFileStorageService, BlobStorageService>();

        return services;
    }
}