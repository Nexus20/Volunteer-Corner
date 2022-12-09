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

        builder.HasMany(x => x.Responses)
            .WithOne(x => x.HelpProposalTo)
            .HasForeignKey(x => x.HelpProposalToId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(x => x.IncludedToHelpRequestResponses)
            .WithOne(x => x.IncludedHelpProposal)
            .HasForeignKey(x => x.IncludedHelpProposalId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}