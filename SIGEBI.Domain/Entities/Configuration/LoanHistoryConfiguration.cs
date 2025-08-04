using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Domain.Entities.Configuration
{
    public class LoanHistoryConfiguration : IEntityTypeConfiguration<LoanHistory>
    {
        public void Configure(EntityTypeBuilder<LoanHistory> builder)
        {
            builder.ToTable("LoanHistories");

            builder.HasKey(e => e.HistoryId);

            builder.Property(e => e.HistoryId)
                .HasColumnName("LoanHistoryId");

            builder.Property(e => e.LoanId)
                .IsRequired();

            builder.Property(e => e.StatusId)
                .IsRequired();

            builder.Property(e => e.ChangedAt)
                .IsRequired();

            builder.Property(e => e.ChangedBy)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.Notes)
                .HasMaxLength(500);

            builder.Property(e => e.FinalStatus)
                .HasMaxLength(100);

            builder.Property(e => e.Observations)
                .HasMaxLength(500);

            builder.HasOne(e => e.Loan)
                .WithMany(e => e.History)
                .HasForeignKey(e => e.LoanId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<LoanStatus>()
                .WithMany()
                .HasForeignKey(e => e.StatusId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
