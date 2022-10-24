using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volunteer_Corner.Data.Entities;

namespace Volunteer_Corner.Data.EntityConfigurations;

internal class HelpProposalConfiguration : IEntityTypeConfiguration<HelpProposal>
{
    public void Configure(EntityTypeBuilder<HelpProposal> builder)
    {
        builder.HasMany(x => x.AdditionalPhotos)
            .WithOne(x => x.HelpProposal)
            .HasForeignKey(x => x.HelpProposalId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}