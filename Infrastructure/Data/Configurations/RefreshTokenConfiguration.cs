using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
   
        public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
        {
            public void Configure(EntityTypeBuilder<RefreshToken> builder)
            {
                builder.ToTable("RefreshToken");

                builder.HasKey(x => x.Id);

                builder.Property(x => x.Token)
                       .IsRequired();

                builder.Property(x => x.Username)
                       .HasMaxLength(100)
                       .IsRequired();

                builder.Property(x => x.Expires)
                       .IsRequired();

                builder.Property(x => x.IsRevoked)
                       .IsRequired();
            }
        }
    }

