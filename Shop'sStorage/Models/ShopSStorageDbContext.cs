using System.Data.Entity;
using System.Windows;

namespace ShopSStorage.Models
{
    public class ShopSStorageDbContext : DbContext
    {
        public ShopSStorageDbContext() : base("name=ShopSStorageConnectionString2")
        {
            //Database.SetInitializer<ShopSStorageDbContext>(new DropCreateDatabaseIfModelChanges<ShopSStorageDbContext>());
        } 
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Cathegory> Cathegories { get; set; }
        public virtual DbSet<SalesHistory> SalesHistories { get; set; }
    }
}
