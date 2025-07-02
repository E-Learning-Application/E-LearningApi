using DATA.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATA.DataAccess.Context.Configurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Content)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(m => m.SentAt)
                .IsRequired();

            builder.Property(m => m.IsRead)
                .IsRequired();

            builder.Property(m => m.IsDeletedBySender)
                .IsRequired();

            builder.Property(m => m.IsDeletedByReceiver)
                .IsRequired();

            builder.HasOne(m => m.Sender)
                .WithMany(u => u.MessagesSent)
                .HasForeignKey(m => m.SenderID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.Receiver)
                .WithMany(u => u.MessagesReceived)
                .HasForeignKey(m => m.ReceiverID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("Messages");
        }
    }
}
