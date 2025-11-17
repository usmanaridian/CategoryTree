using CategoryTree.Core;
using Microsoft.EntityFrameworkCore;
using System;

namespace CategoryTree.Infrastructure
{
    public class CategoryDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }

        public CategoryDbContext(DbContextOptions<CategoryDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
            .ToTable("Category");
        }
    }
}
