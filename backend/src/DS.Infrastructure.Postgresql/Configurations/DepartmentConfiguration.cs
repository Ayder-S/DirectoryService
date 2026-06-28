using CSharpFunctionalExtensions;
using DS.Domain.Entities;
using DS.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Kernel.Constants;
using Path = DS.Domain.ValueObjects.Path;

namespace DS.Infrastructure.Postgresql.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    { 
        builder.ToTable("departments");
        
        builder.HasKey(d => d.Id)
            .HasName("pk_departments");

        builder.Property(d => d.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(d => d.Name)
            .HasColumnName("name")
            .HasConversion(
                dn => dn.Value,
                s => Name.ReadName(s))
            .HasMaxLength(LengthConstants.Name.MAX_LENGTH)
            .IsRequired();
        
        builder.Property(d => d.Identifier)
            .HasColumnName("identifier")
            .HasConversion(
                di => di.Value,
                s => Identifier.ReadIdentifier(s))
            .HasMaxLength(LengthConstants.Identifier.MAX_LENGTH)
            .IsRequired();

        builder.Property(d => d.ParentId)
            .HasColumnName("parent_id")
            .IsRequired(false);
        
        builder.Property(d => d.Path)
            .HasColumnName("path")
            .HasConversion(
                dp => dp.Value,
                s => Path.ReadPath(s))
            .IsRequired();
        
        builder.Property(d => d.Depth)
            .HasColumnName("depth")
            .HasConversion(
                depth => depth.Value,
                value => Depth.FromValue(value))
            .IsRequired();
        
        builder.Property(d => d.IsActive)
            .HasColumnName("is_active")
            .IsRequired();
        
        builder.Property(d => d.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();
        
        builder.Property(d => d.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();
    }
}