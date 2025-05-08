using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Entities
{
    public class FileEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public int OwnerId { get; set; }
        public DateTime UploadDate { get; set; }
    }
    public class FileConfiguration : IEntityTypeConfiguration<FileEntity>
    {
        public void Configure(EntityTypeBuilder<FileEntity> builder)
        {
            builder.HasKey(f => f.Id);

            builder.Property(f => f.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(f => f.Description)
                .HasMaxLength(500);

            builder.Property(f => f.UploadDate)
                .IsRequired();
            builder.HasMany<FileShareEntity>()
                .WithOne()
                .HasForeignKey(fs => fs.FileId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
