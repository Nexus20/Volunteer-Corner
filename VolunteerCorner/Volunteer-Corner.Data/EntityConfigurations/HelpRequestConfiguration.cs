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
    }
}