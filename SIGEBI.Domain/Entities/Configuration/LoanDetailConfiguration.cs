using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SIGEBI.Domain.Entities.Configuration
{
    public class LoanDetailConfiguration : IEntityTypeConfiguration<LoanDetail>
    {
        public void Configure(EntityTypeBuilder<LoanDetail> builder)
        {
            builder.HasKey(e => e.LoanDetailId);

            builder.Property(e => e.CreatedAt)
                .HasColumnType("datetime2(7)")
                .IsRequired();

            builder.Property(e => e.CreatedBy)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.UpdatedAt)
                .HasColumnType("datetime2(7)");

            builder.Property(e => e.UpdatedBy)
                .HasMaxLength(100);

            builder.Property(e => e.IsDeleted)
                .IsRequired();

            builder.Property(e => e.DeletedAt)
                .HasColumnType("datetime2(7)");

            builder.Property(e => e.DeletedBy)
                .HasMaxLength(100);

            builder.HasOne(e => e.Loan)
                .WithMany(e => e.LoanDetails)
                .HasForeignKey(e => e.LoanId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Book)
                .WithMany()
                .HasForeignKey(e => e.BookId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
