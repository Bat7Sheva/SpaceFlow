using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpaceFlow.Api.Models;

namespace SpaceFlow.Api.Data.Configurations;

public class InteractionConfiguration : IEntityTypeConfiguration<Interaction>
{
    public void Configure(EntityTypeBuilder<Interaction> builder)
    {
        builder.ToTable("Interactions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Channel)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Summary)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(x => x.InteractionAt)
            .IsRequired();
    }
}