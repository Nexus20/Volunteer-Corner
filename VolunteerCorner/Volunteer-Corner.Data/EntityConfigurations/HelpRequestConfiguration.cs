using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volunteer_Corner.Data.Entities;

namespace Volunteer_Corner.Data.EntityConfigurations;

internal class HelpRequestConfiguration : IEntityTypeConfiguration<HelpRequest>
{
    public void Configure(EntityTypeBuilder<HelpRequest> builder)
    {
        builder.HasMany(x => x.AdditionalDocuments)
            .WithOne(x => x.HelpRequest)
            .HasForeignKey(x => x.HelpRequestId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(x => x.Responses)
            .WithOne(x => x.HelpRequestTo)
            .HasForeignKey(x => x.HelpRequestToId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasMany(x => x.IncludedToHelpProposalResponses)
            .WithOne(x => x.IncludedHelpRequest)
            .HasForeignKey(x => x.IncludedHelpRequestId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}