using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace LB11._2
{
    class ShopContext:DbContext
    {
        public ShopContext() : base("DBConnection")
        {
        }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Collection>().HasMany(c => c.Items).WithOptional(c => c.Collection).HasForeignKey(c=>c.CollectionId).WillCascadeOnDelete(true);
            modelBuilder.Entity<Item>().HasMany<Material>(c => c.Materials)
                .WithMany(c => c.Items);
            modelBuilder.Entity<Material>().HasMany<Supplier>(c => c.Suppliers).WithMany(c => c.Materials);
            modelBuilder.Entity<Item>().ToTable("Stock");

            modelBuilder.Entity<Supplier>().Property(p => p.name).HasColumnName("Company name");
            modelBuilder.Entity<Supplier>().Property(p => p.name).IsRequired();
            modelBuilder.Entity<Supplier>().Property(p => p.name).HasMaxLength(30);

            modelBuilder.Entity<Collection>().Property(p => p.st_date).HasColumnType("date");
            modelBuilder.Entity<Collection>().Property(p => p.end_date).HasColumnType("date");
        }
    }



    public static class EntityExtensions
    {
        public static void Clear<T>(this DbSet<T> dbSet) where T : class
        {
            dbSet.RemoveRange(dbSet);
        }
    }
}
