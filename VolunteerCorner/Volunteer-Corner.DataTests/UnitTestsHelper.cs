using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Volunteer_Corner.Data;
using Volunteer_Corner.Data.Entities;
using Volunteer_Corner.Data.Entities.Identity;
using Volunteer_Corner.Data.Enums;

namespace Volunteer_Corner.DataTests;

internal static class UnitTestsHelper
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
        if (!context.Users.Any())
        {
            context.Users.AddRange(
                new User()
                {
                    Id = "1",
                    FirstName = "User 1 FirstName",
                    LastName = "User 1 LastName",
                    Email = "user1@example.com",
                },
                new User()
                {
                    Id = "2",
                    FirstName = "User 2 FirstName",
                    LastName = "User 2 LastName",
                    Email = "user2@example.com",
                },
                new User()
                {
                    Id = "3",
                    FirstName = "User 3 FirstName",
                    LastName = "User 3 LastName",
                    Email = "user3@example.com",
                }
            );
        }
        
        if (!context.HelpSeekers.Any())
        {
            context.HelpSeekers.AddRange(
                new HelpSeeker()
                {
                    Id = "1",
                    UserId = "1"
                },
                new HelpSeeker()
                {
                    Id = "2",
                    UserId = "2"
                },
                new HelpSeeker()
                {
                    Id = "3",
                    UserId = "3"
                }
            );
        }

        if (!context.HelpRequests.Any())
        {
            context.HelpRequests.AddRange(
                new HelpRequest()
                {
                    Id = "1",
                    Name = "Help request 1 Name",
                    Description = "Help request 1 Description",
                    Status = HelpRequestStatus.Active,
                    OwnerId = "1",
                    Location = "Location"
                },
                new HelpRequest()
                {
                    Id = "2",
                    Name = "Help request 2 Name",
                    Description = "Help request 2 Description",
                    Status = HelpRequestStatus.Active,
                    OwnerId = "2",
                    Location = "Location"
                },
                new HelpRequest()
                {
                    Id = "3",
                    Name = "Help request 3 Name",
                    Description = "Help request 3 Description",
                    Status = HelpRequestStatus.Active,
                    OwnerId = "3",
                    Location = "Location"
                }
            );
        }
        if (!context.HelpRequestDocuments.Any())
        {
            context.HelpRequestDocuments.AddRange(
                new HelpRequestDocument()
                {
                    Id = "1",
                    FilePath = "Path",
                    HelpRequestId = "1"
              
                },
                new HelpRequestDocument()
                {
                    Id = "2",
                    FilePath = "Path",
                    HelpRequestId = "1"

                },
                new HelpRequestDocument()
                {
                    Id = "3",
                    FilePath = "Path",
                    HelpRequestId = "1"

                }
            );
        }

        context.SaveChanges();
    }
}