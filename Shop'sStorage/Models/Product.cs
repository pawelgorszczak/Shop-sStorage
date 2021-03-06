﻿using System.Collections.Generic;
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
        public decimal DetailPrice { get; set; }
        public virtual Cathegory Cathegory { get; set; }
        [Timestamp]
        public byte[] RowVersionBytes { get; set; }

        public bool IsSelected { get; set; }

        public virtual ICollection<SalesHistory> SalesHistories { get; set; }

    }
}