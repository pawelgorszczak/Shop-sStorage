using System.ComponentModel.DataAnnotations;

namespace ShopSStorage.Models
{
    public class Product
    {
        public Product()
        {
            
        }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int StorageAmount { get; set; }
        public int DetailPrice { get; set; }
        public virtual Cathegory Cathegory { get; set; }
        [Timestamp]
        public byte[] RowVersionBytes { get; set; }

    }
}