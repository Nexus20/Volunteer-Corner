using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volunteer_Corner.Data.Entities;

namespace Volunteer_Corner.Data.EntityConfigurations;

internal class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
{
    public void Configure(EntityTypeBuilder<Volunteer> builder)
    {
        builder.HasOne(x => x.User)
            .WithOne(x => x.Volunteer)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.HelpProposals)
            .WithOne(x => x.Owner)
            .HasForeignKey(x => x.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}