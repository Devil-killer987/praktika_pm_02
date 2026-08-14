using Microsoft.EntityFrameworkCore;
using SchoolEquipmentApi.Models;

namespace SchoolEquipmentApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Building> Buildings { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<EquipmentType> EquipmentTypes { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<SpecificationCategory> SpecificationCategories { get; set; }
        public DbSet<EquipmentSpecification> EquipmentSpecifications { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Equipment - InventoryNumber unique
            modelBuilder.Entity<Equipment>()
                .HasIndex(e => e.InventoryNumber)
                .IsUnique();

            // User - Login unique
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Login)
                .IsUnique();

            // Relationships
            modelBuilder.Entity<Classroom>()
                .HasOne(c => c.Building)
                .WithMany(b => b.Classrooms)
                .HasForeignKey(c => c.BuildingId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Equipment>()
                .HasOne(e => e.Classroom)
                .WithMany(c => c.Equipment)
                .HasForeignKey(e => e.ClassroomId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Equipment>()
                .HasOne(e => e.EquipmentType)
                .WithMany(t => t.Equipment)
                .HasForeignKey(e => e.EquipmentTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EquipmentSpecification>()
                .HasOne(es => es.Equipment)
                .WithMany(e => e.Specifications)
                .HasForeignKey(es => es.EquipmentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EquipmentSpecification>()
                .HasOne(es => es.Category)
                .WithMany(c => c.Specifications)
                .HasForeignKey(es => es.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<SpecificationCategory>()
                .HasOne(sc => sc.EquipmentType)
                .WithMany(et => et.SpecificationCategories)
                .HasForeignKey(sc => sc.EquipmentTypeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}