using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volunteer_Corner.Data.Entities;
using Volunteer_Corner.Data.Entities.Abstract;
using Volunteer_Corner.Data.Entities.Identity;

namespace Volunteer_Corner.Data;

public class ApplicationDbContext : IdentityDbContext<User, Role, string, IdentityUserClaim<string>, UserRole,
    IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
{
    public DbSet<UserDocument> UserDocuments { get; set; }
    
    public DbSet<HelpSeeker> HelpSeekers { get; set; }
    public DbSet<HelpRequest> HelpRequests { get; set; }
    public DbSet<HelpRequestDocument> HelpRequestDocuments { get; set; }
    
    public DbSet<Volunteer> Volunteers { get; set; }
    public DbSet<HelpProposal> HelpProposals { get; set; }
    public DbSet<HelpProposalPhoto> HelpProposalPhotos { get; set; }

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
        if (!Database.IsInMemory())
        {
            Database.Migrate();
        }
    }

    public override int SaveChanges()
    {
        AddInfoBeforeUpdate();
        return base.SaveChanges();
    }

    public Task<int> SaveChangesAsync()
    {
        AddInfoBeforeUpdate();
        return base.SaveChangesAsync();
    }

    private void AddInfoBeforeUpdate()
    {
        var entries = ChangeTracker.Entries()
            .Where(x => x.Entity is BaseEntity && x.State is EntityState.Added or EntityState.Modified);
        
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                ((BaseEntity)entry.Entity).CreatedDate = DateTime.UtcNow;
            }
            ((BaseEntity)entry.Entity).LastModifiedDate = DateTime.UtcNow;
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(ApplicationDbContext))!);
    }
}