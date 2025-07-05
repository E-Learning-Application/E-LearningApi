using DATA.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATA.DataAccess.Context.Configurations
{
    public class LanguageConfiguration : IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);

            builder.Property(x => x.Code).IsRequired().HasMaxLength(10);

            builder.HasMany(x => x.LanguagePreferences)
                .WithOne(lp => lp.Language)
                .HasForeignKey(lp => lp.LanguageId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("Languages");
        }
    }
}
