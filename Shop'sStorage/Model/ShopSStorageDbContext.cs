using System.Data.Entity;
using System.Windows;

namespace ShopSStorage.Model
{
    internal class ShopSStorageDbContext : DbContext
    {
        public ShopSStorageDbContext() : base("name=ShopSStorageConnectionString")
        {
            MessageBox.Show("test");
            Database.SetInitializer<ShopSStorageDbContext>(new DropCreateDatabaseIfModelChanges<ShopSStorageDbContext>());
        }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Cathegory> Cathegories { get; set; }
    }
}
