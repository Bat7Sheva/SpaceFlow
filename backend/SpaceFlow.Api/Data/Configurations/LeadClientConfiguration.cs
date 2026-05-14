using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpaceFlow.Api.Models;

namespace SpaceFlow.Api.Data.Configurations;

public class LeadClientConfiguration : IEntityTypeConfiguration<LeadClient>
{
    public void Configure(EntityTypeBuilder<LeadClient> builder)
    {
        builder.ToTable("LeadsClients");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.FullName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Phone)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Email)
            .HasMaxLength(200);

        builder.Property(x => x.Source)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Notes)
            .HasMaxLength(2000);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .IsRequired();

        builder.HasMany(x => x.Interactions)
            .WithOne(x => x.LeadClient)
            .HasForeignKey(x => x.LeadClientId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}