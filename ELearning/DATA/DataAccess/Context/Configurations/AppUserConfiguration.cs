using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DATA.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATA.DataAccess.Context.Configurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ImagePath).IsRequired(false);
            builder.Property(x => x.Bio).IsRequired(false);
            builder.Property(x=>x.IsOnline).IsRequired().HasDefaultValue(false);
            builder.HasMany(x=>x.UserInterests)
                .WithOne(ui=>ui.User)
                .HasForeignKey(ui=>ui.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x=>x.MatchesAsUser1)
                .WithOne(um=>um.User1)
                .HasForeignKey(um=>um.UserId1)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(x=>x.MatchesAsUser2)
                .WithOne(um=>um.User2)
                .HasForeignKey(um=>um.UserId2)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
