using Gochs.ProtectiveStructures.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gochs.ProtectiveStructures.Data.Configurations;

public class ProtectiveStructureConfiguration : IEntityTypeConfiguration<ProtectiveStructure>
{
    public void Configure(EntityTypeBuilder<ProtectiveStructure> builder)
    {
        builder.ToTable("ProtectiveStructures",
            table => table.HasCheckConstraint(
                "CK_ProtectiveStructures_Capacity_Positive",
                "\"Capacity\" > 0"));

        builder.HasKey(x => x.Id);

        builder.Property(x => x.RegistrationNumber)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(x => x.RegistrationNumber)
            .IsUnique();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Type)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Address)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(x => x.Capacity)
            .IsRequired();

        builder.Property(x => x.Condition)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.ResponsiblePerson)
            .HasMaxLength(200);

        builder.Property(x => x.Phone)
            .HasMaxLength(30);

        builder.Property(x => x.Notes)
            .HasMaxLength(1000);

        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasMany(x => x.Employees)
            .WithOne(x => x.ProtectiveStructure)
            .HasForeignKey(x => x.ProtectiveStructureId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Inspections)
            .WithOne(x => x.ProtectiveStructure)
            .HasForeignKey(x => x.ProtectiveStructureId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
