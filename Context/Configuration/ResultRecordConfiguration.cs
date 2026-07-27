using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Context.Configuration;

public class ResultRecordConfiguration : IEntityTypeConfiguration<ResultRecord>
{
    public void Configure(EntityTypeBuilder<ResultRecord> builder)
    {
        builder.ToTable("Results");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.FileName)
            .IsRequired()
            .HasMaxLength(255);

        // Файл с таким именем уже существует -> перезаписываем значения,
        // поэтому имя файла должно быть уникальным
        builder.HasIndex(r => r.FileName)
            .IsUnique();

        builder.Property(r => r.DeltaSeconds).IsRequired();
        builder.Property(r => r.MinDate).IsRequired();
        builder.Property(r => r.AverageExecutionTime).IsRequired();
        builder.Property(r => r.AverageValue).IsRequired();
        builder.Property(r => r.MedianValue).IsRequired();
        builder.Property(r => r.MaxValue).IsRequired();
        builder.Property(r => r.MinValue).IsRequired();
    }
}