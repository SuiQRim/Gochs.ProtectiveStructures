using Gochs.ProtectiveStructures.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gochs.ProtectiveStructures.Data.Configurations;

public class InspectionConfiguration : IEntityTypeConfiguration<Inspection>
{
    public void Configure(EntityTypeBuilder<Inspection> builder)
    {
        builder.ToTable("Inspections");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.InspectionDate)
            .IsRequired();

        builder.Property(x => x.InspectorName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.ResultCondition)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Findings)
            .HasMaxLength(1000);

        builder.Property(x => x.RequiredActions)
            .HasMaxLength(1000);

        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}
