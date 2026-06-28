using DS.Domain.Entities;
using DS.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Kernel.Constants;

namespace DS.Infrastructure.Postgresql.Configurations;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    { 
        builder.ToTable("locations");
        
        builder.HasKey(l => l.Id)
            .HasName("pk_locations");

        builder.Property(l => l.Id)
            .HasColumnName("id");
        
        builder.Property(l => l.Name)
            .HasColumnName("name")
            .HasConversion(
                ln => ln.Value,
                s => Name.ReadName(s))
            .HasMaxLength(LengthConstants.Name.MAX_LENGTH)
            .IsRequired();

        builder.OwnsOne(l => l.Address, ab =>
        {
            ab.ToJson("address");

            ab.Property(a => a.Country)
                .HasJsonPropertyName("country")
                .IsRequired();

            ab.Property(a => a.Region)
                .HasJsonPropertyName("region")
                .IsRequired();

            ab.Property(a => a.City)
                .HasJsonPropertyName("city")
                .IsRequired();

            ab.Property(a => a.Street)
                .HasJsonPropertyName("street")
                .IsRequired(false);

            ab.Property(a => a.Building)
                .HasJsonPropertyName("building")
                .IsRequired(false);
        });

        builder.Property(l => l.Timezone)
            .HasColumnName("timezone")
            .HasConversion(
                lt => lt.Value,
                s => Timezone.ReadTimezone(s))
            .IsRequired();
        
        builder.Property(l => l.IsActive)
            .HasColumnName("is_active")
            .IsRequired();

        builder.Property(l => l.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();
        
        builder.Property(l => l.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();


    }
}