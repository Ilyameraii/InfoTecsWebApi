using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Context.Configuration;

public class ValueRecordConfiguration : IEntityTypeConfiguration<ValueRecord>
{
    public void Configure(EntityTypeBuilder<ValueRecord> builder)
    {
        builder.ToTable("Values");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.FileName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(v => v.Date)
            .IsRequired();

        builder.Property(v => v.ExecutionTime)
            .IsRequired();

        builder.Property(v => v.Value)
            .IsRequired();

        builder.HasIndex(v => new { v.FileName, v.Date });
    }
}