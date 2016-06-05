using System.Data.Entity;
using System.Windows;

namespace ShopSStorage.Models
{
    public class ShopSStorageDbContext : DbContext
    {
        public ShopSStorageDbContext() : base("name=ShopSStorageConnectionString")
        {
        }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Cathegory> Cathegories { get; set; }
    }
}
