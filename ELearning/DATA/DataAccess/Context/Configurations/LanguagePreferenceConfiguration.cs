using DATA.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATA.DataAccess.Context.Configurations
{
    public class LanguagePreferenceConfiguration : IEntityTypeConfiguration<LanguagePreference>
    {
        public void Configure(EntityTypeBuilder<LanguagePreference> builder)
        {
            builder.HasKey(lp => new { lp.UserId, lp.LanguageId });
            builder.Property(lp => lp.ProficiencyLevel).IsRequired();
            builder.Property(lp => lp.IsLearning).IsRequired();

            builder.HasOne(lp => lp.Language)
                .WithMany(l => l.LanguagePreferences)
                .HasForeignKey(lp => lp.LanguageId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(lp => lp.AppUser)
                .WithMany(u => u.LanguagePreferences)
                .HasForeignKey(lp => lp.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("LanguagePreferences");
        }
    }
}
