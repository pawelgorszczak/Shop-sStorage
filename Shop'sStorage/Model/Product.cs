using System.ComponentModel.DataAnnotations;


namespace ShopSStorage.Model
{
    public class Product
    {
        public Product()
        {
            
        }

        public int ProductId { get; set; }

        [Required]
        [StringLength(100)]
        public string ProductName { get; set; }

        [Required]
        public int StorageAmount { get; set; }
        
        public virtual Cathegory Cathegory { get; set; }

        [Timestamp]
        public byte[] RowVersionBytes { get; set; }

    }
}