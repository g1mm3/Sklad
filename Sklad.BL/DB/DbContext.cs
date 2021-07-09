using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Sklad.BL.DB
{
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<ContainerType> ContainerTypes { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Container> Containers { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbContext()
        {
            // Для первого запуска базы данных и ее создании по модели
            // Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Локальная база данных - !products.db
            optionsBuilder.UseSqlite("Filename=!products.db");
        }
    }
}
