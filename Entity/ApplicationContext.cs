using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstProject.Entity;
using Microsoft.EntityFrameworkCore;

namespace FirstProject
{
    class ApplicationContext : DbContext
    {
        public DbSet<MaterialType> MaterialTypes { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<ProductMaterial> ProductMaterials { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies().UseSqlServer("Server=DESKTOP-LNREVUA\\SQLEXPRESS;Database=AromaDom;Trusted_Connection=True;TrustServerCertificate=true");
        }
    }
}
