using ABP.AppCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace ABP.AppCore
{
    public class ABPContext : DbContext
    {
        public DbSet<DeviceEntity> Devices { get; set; }

        public DbSet<DeviceTokenEntity> DeviceTokens { get; set; }

        public DbSet<ExperimentEntity> Experiments { get; set; }

        public ABPContext(DbContextOptions<ABPContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");
            modelBuilder.Entity<DeviceEntity>(entity =>
            {
                entity.Property(e => e.DeviceName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<DeviceTokenEntity>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Value
                    )
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<ExperimentEntity>(entity =>
            {
                entity.Property(e => e.KeyValue)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Value
                    )
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasKey(e => e.Id);
            });
        }
    }
}
