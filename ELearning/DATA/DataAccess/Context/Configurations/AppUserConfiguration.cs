using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DATA.Models;
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
            builder.HasMany(x => x.LanguagePreferences).WithMany(x => x.LanguagePreferences)
                .UsingEntity<LanguagePreference>(
                    j => j
                        .HasOne(l => l.Language)
                        .WithMany()
                        .HasForeignKey(l => l.LanguageId),
                    j => j
                        .HasOne(u => u.AppUser)
                        .WithMany()
                        .HasForeignKey(u => u.UserId),
                    j =>
                    {
                        j.HasKey(t => new { t.UserId, t.LanguageId });
                    });
        }
    }
}
