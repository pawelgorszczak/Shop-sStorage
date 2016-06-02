using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShopSStorage.Model
{
    public class Cathegory
    {
        public Cathegory()
        {
            
        }
        public int CathegoryId { get; set; }
        [Required]
        [StringLength(30)]
        public string CathegoryName { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
    
}
