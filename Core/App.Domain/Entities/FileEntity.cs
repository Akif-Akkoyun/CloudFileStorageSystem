using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using App.Domain.Enums;

namespace App.Domain.Entities
{
    public class FileEntity
    {
        public int Id { get; set; }
        public string FileName { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string FilePath { get; set; } = default!;
        public int OwnerId { get; set; }
        public DateTime UploadDate { get; set; }
        public FileVisibility Visibility { get; set; } = FileVisibility.Private;
        public ICollection<FileShareEntity> FileShares { get; set; } = new List<FileShareEntity>();
    }
    public class FileConfiguration : IEntityTypeConfiguration<FileEntity>
    {
        public void Configure(EntityTypeBuilder<FileEntity> builder)
        {
            builder.HasKey(f => f.Id);
            builder.Property(f => f.FileName)
                .IsRequired()
                .HasMaxLength(200);
            builder.Property(f => f.Description)
                .HasMaxLength(500);
            builder.Property(f => f.UploadDate)
                .IsRequired();
            builder.Property(f => f.Visibility)
                .HasConversion<string>()
                .IsRequired();
            builder.HasMany<FileShareEntity>()
                .WithOne()
                .HasForeignKey(fs => fs.FileId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Property(f => f.FilePath)
                .IsRequired()
                .HasMaxLength(500);
        }
    }
}
