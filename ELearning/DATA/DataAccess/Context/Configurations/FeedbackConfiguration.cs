using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using DATA.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DATA.DataAccess.Context.Configurations
{
    public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
    {
        public void Configure(EntityTypeBuilder<Feedback> builder)
        {
            builder.HasOne(f => f.Feedbacker)
                .WithMany(u => u.GivenFeedbacks)
                .HasForeignKey(f => f.FeedbackerId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            builder.HasOne(f => f.Feedbacked)
                .WithMany(u => u.ReceivedFeedbacks)
                .HasForeignKey(f => f.FeedbackedId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            // Ensure a user cannot feedback themselves
            builder.HasCheckConstraint("CK_Feedback_NoSelfFeedback", "FeedbackerId != FeedbackedId");
        }
    }
}
