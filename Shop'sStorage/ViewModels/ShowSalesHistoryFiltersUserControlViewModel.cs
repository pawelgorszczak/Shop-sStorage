using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmSchemats;
using ShopSStorage.Models;

namespace ShopSStorage.ViewModels
{
    public class ShowSalesHistoryFiltersUserControlViewModel : ViewModel
    {
        private BusinessDbContext _context;

        public ShowSalesHistoryFiltersUserControlViewModel(BusinessDbContext context)
        {
            _context = context;
        }
    }
}
