using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIGEBI.Domain.Entities;

namespace SIGEBI.Domain.Entities.Configuration
{
    public class LoanStatusConfiguration : IEntityTypeConfiguration<LoanStatus>
    {
        public void Configure(EntityTypeBuilder<LoanStatus> builder)
        {
            builder.ToTable("LoanStatuses");

            builder.HasKey(e => e.StatusId);

            builder.Property(e => e.StatusId)
                .HasColumnName("LoanStatusId");

            builder.Property(e => e.StatusName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Description)
                .HasMaxLength(250);

            builder.Property(e => e.CreatedAt)
                .IsRequired();

            builder.Property(e => e.CreatedBy)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.UpdatedAt);
            builder.Property(e => e.UpdatedBy).HasMaxLength(100);

            builder.Property(e => e.IsDeleted)
                .IsRequired();

            builder.Property(e => e.DeletedAt);
            builder.Property(e => e.DeletedBy).HasMaxLength(100);

            builder.HasMany(e => e.Loans)
                .WithOne(e => e.LoanStatus)
                .HasForeignKey(e => e.LoanStatusId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
