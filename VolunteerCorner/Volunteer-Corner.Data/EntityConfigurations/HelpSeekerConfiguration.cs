using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volunteer_Corner.Data.Entities;

namespace Volunteer_Corner.Data.EntityConfigurations;

internal class HelpSeekerConfiguration : IEntityTypeConfiguration<HelpSeeker>
{
    public void Configure(EntityTypeBuilder<HelpSeeker> builder)
    {
        builder.HasOne(x => x.User)
            .WithOne(x => x.HelpSeeker)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.HelpRequests)
            .WithOne(x => x.Owner)
            .HasForeignKey(x => x.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.HelpProposalResponses)
            .WithOne(x => x.HelpSeekerFrom)
            .HasForeignKey(x => x.HelpSeekerFromId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}