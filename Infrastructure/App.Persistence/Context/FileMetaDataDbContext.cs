using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Persistence.Context
{
    internal class FileMetaDataDbContext : DbContext
    {
        public FileMetaDataDbContext(DbContextOptions<FileMetaDataDbContext> options) : base(options)
        {
        }
        public DbSet<FileEntity> Files { get; set; }
        public DbSet<FileShareEntity> FileShares { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FileConfiguration());
            modelBuilder.ApplyConfiguration(new FileShareConfiguration());
        }
    }
}
