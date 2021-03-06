﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShopSStorage.Models
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

        public bool IsSelected { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
    
}
