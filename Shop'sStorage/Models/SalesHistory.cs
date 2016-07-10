using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ShopSStorage.Models
{
    public class SalesHistory
    {
        public SalesHistory()
        {
            
        }
        public int SalesHistoryId { get; set; }
        public int SoldAmount { get; set; }
        public DateTime SoldDateTime { get; set; }
        public virtual Product Product { get; set; }
    }
}
