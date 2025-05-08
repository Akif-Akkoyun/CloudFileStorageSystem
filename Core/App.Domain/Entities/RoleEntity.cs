using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Entities
{
    public class RoleEntity
    {
        public int Id { get; set; }
        public string RoleName { get; set; } = default!;

        // Navigation
        public ICollection<UserEntity> Users { get; set; } = new List<UserEntity>();
    }
    public class RoleConfiguration : IEntityTypeConfiguration<RoleEntity>
    {
        public void Configure(EntityTypeBuilder<RoleEntity> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.RoleName)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
