﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Moq;
using Volunteer_Corner.Business.Mappings;
using Volunteer_Corner.Data;
using Volunteer_Corner.Data.Entities.Identity;

namespace Volunteer_Corner.BusinessTests;

public static class UnitTestsHelper
{
    public static IConfiguration GetConfiguration()
    {
        // Add here needed configuration to mock appsettings.json file
        var inMemorySettings = new Dictionary<string, string> {
            {"SectionName:SomeKey", "SectionValue"},
        };

        return new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();
    }
    
    public static Mock<UserManager<User>> GetUserManagerMock() {
        var store = new Mock<IUserStore<User>>();
        var manager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        manager.Object.UserValidators.Add(new UserValidator<User>());
        manager.Object.PasswordValidators.Add(new PasswordValidator<User>());
        return manager;
    }
    
    public static Mapper GetMapper()
    {
        var profile = new AutomapperBusinessProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));

        return new Mapper(configuration);
    }
    
    public static DbContextOptions<ApplicationDbContext> GetUnitTestDbOptions()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        using var context = new ApplicationDbContext(options);
        SeedData(context);

        return options;
    }

    private static void SeedData(ApplicationDbContext context)
    {
        // Seed the needed data here into inMemory Db context
    }
}