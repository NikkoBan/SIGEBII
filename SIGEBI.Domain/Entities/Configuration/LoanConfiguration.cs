using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace SIGEBI.Domain.Entities.Configuration
{
    public class LoanConfiguration : IEntityTypeConfiguration<Loan>
    {
        public void Configure(EntityTypeBuilder<Loan> builder)
        {
            builder.HasKey(e => e.LoanId);

            builder.Property(e => e.LoanDate)
                .IsRequired();

            builder.Property(e => e.DueDate)
                .IsRequired();

            builder.Property(e => e.ReturnDate)
                .IsRequired(false);

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

            builder.HasOne(e => e.Book)
                .WithMany()
                .HasForeignKey(e => e.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.LoanStatus)
                .WithMany()
                .HasForeignKey(e => e.LoanStatusId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}