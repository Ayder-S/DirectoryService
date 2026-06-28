using DS.Domain.Entities;
using DS.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Kernel.Constants;

namespace DS.Infrastructure.Postgresql.Configurations;

public class PositionConfiguration : IEntityTypeConfiguration<Position>
{
    public void Configure(EntityTypeBuilder<Position> builder)
    { 
        builder.ToTable("positions");
        
        builder.HasKey(p => p.Id).HasName("pk_positions");
        
        builder.Property(p => p.Name)
            .HasColumnName("name")
            .HasConversion(
                pn => pn.Value,
                s => Name.ReadName(s))
            .HasMaxLength(LengthConstants.Name.MAX_LENGTH)
            .IsRequired();

        builder.Property(p => p.Description)
            .HasColumnName("description")
            .HasConversion(
                pb => pb.Value,
                s => Description.ReadDescription(s))
            .IsRequired(false);
        
        builder.Property(p => p.IsActive)
            .HasColumnName("is_active")
            .IsRequired();

        builder.Property(p => p.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();
        
        builder.Property(p => p.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();
    }
}