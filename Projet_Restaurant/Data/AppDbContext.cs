using Microsoft.EntityFrameworkCore;
using Projet_Restaurant.Model;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Projet_Restaurant.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Ajoutez vos DbSet ici
        public DbSet<Category> Categories { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> Details { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ici, vous pouvez ajouter ou trouver le code de configuration
            modelBuilder.Entity<Category>()
                .Property(c => c.CategoryId)
                .ValueGeneratedOnAdd();
        }

    }

}


