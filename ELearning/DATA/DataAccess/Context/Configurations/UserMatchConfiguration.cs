using DATA.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA.DataAccess.Context.Configurations
{
    public class UserMatchConfiguration:IEntityTypeConfiguration<UserMatch>
    {
        public void Configure(EntityTypeBuilder<UserMatch> builder)
        {
            builder.HasKey(um => um.Id);

            builder.Property(um => um.MatchType)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(um => um.MatchScore)
                .IsRequired()
                .HasColumnType("float");

            builder.Property(um => um.CreatedAt)
                .IsRequired();

            builder.Property(um => um.IsActive)
                .IsRequired();

            builder.HasOne(um => um.User1)
                .WithMany(u => u.MatchesAsUser1)
                .HasForeignKey(um => um.UserId1)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(um => um.User2)
                .WithMany(u => u.MatchesAsUser2)
                .HasForeignKey(um => um.UserId2)
                .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("UserMatches");
        }
    }
}
