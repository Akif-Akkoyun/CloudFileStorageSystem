using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Entities
{
    public class FileShareEntity
    {
        public int Id { get; set; }
        public int FileId { get; set; }
        public int UserId { get; set; }
        public string Permission { get; set; } = default!;
    }
    public class FileShareConfiguration : IEntityTypeConfiguration<FileShareEntity>
    {
        public void Configure(EntityTypeBuilder<FileShareEntity> builder)
        {
            builder.HasKey(fs => new { fs.FileId, fs.UserId });

            builder.Property(fs => fs.Permission)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
