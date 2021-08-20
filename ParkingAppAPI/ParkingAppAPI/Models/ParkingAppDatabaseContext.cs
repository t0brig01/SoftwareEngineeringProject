using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ParkingAppAPI.Models
{
    public partial class ParkingAppDatabaseContext : DbContext
    {
        public ParkingAppDatabaseContext()
        {
        }

        public ParkingAppDatabaseContext(DbContextOptions<ParkingAppDatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admin { get; set; }
        public virtual DbSet<ParkingLot> ParkingLot { get; set; }
        public virtual DbSet<ParkingMap> ParkingMap { get; set; }
        public virtual DbSet<ParkingPassCd> ParkingPassCd { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer("Server=tcp:parkingappdbserver.database.windows.net,1433;Initial Catalog=ParkingAppDatabase;Persist Security Info=False;User ID=ParkingAppAdmin;Password=P@ssw0rd;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Admin>(entity =>
            {
                entity.ToTable("admin");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ParkingLot>(entity =>
            {
                entity.ToTable("parking_lot");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasColumnName("address");

                entity.Property(e => e.NoPassStart).HasColumnName("no_pass_start");

                entity.Property(e => e.AnyPassStart).HasColumnName("any_pass_start");

                entity.Property(e => e.ExclusivePassStart).HasColumnName("exclusive_pass_start");

                entity.Property(e => e.ShortDesc)
                    .IsRequired()
                    .HasColumnName("short_desc");

                entity.Property(e => e.WeekendEnforcementFlag).HasColumnName("weekend_enforcement_flag");
            });

            modelBuilder.Entity<ParkingMap>(entity =>
            {
                entity.ToTable("parking_map");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ParkingLotId).HasColumnName("parking_lot_id");

                entity.Property(e => e.ParkingPassCd)
                    .IsRequired()
                    .HasColumnName("parking_pass_cd")
                    .HasMaxLength(50);

                entity.Property(e => e.PrimaryFlag).HasColumnName("primary_flag");

                entity.HasOne(d => d.ParkingLot)
                    .WithMany(p => p.ParkingMap)
                    .HasForeignKey(d => d.ParkingLotId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_parking_map_parking_lot");

                entity.HasOne(d => d.ParkingPassCdNavigation)
                    .WithMany(p => p.ParkingMap)
                    .HasForeignKey(d => d.ParkingPassCd)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_parking_map_parking_pass_cd");
            });

            modelBuilder.Entity<ParkingPassCd>(entity =>
            {
                entity.HasKey(e => e.ParkingPassCd1);

                entity.ToTable("parking_pass_cd");

                entity.Property(e => e.ParkingPassCd1)
                    .HasColumnName("parking_pass_cd")
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.ActiveFlag).HasColumnName("active_flag");

                entity.Property(e => e.DisplaySequence).HasColumnName("display_sequence");

                entity.Property(e => e.ShortDesc)
                    .IsRequired()
                    .HasColumnName("short_desc")
                    .HasMaxLength(50);
            });
        }
    }
}
