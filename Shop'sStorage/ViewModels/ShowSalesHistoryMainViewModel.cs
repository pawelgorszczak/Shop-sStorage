using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MvvmSchemats;
using ShopSStorage.Models;
using ShopSStorage.Views.ShowSalesHistory;

namespace ShopSStorage.ViewModels
{
    public class ShowSalesHistoryMainViewModel : ViewModel
    {
        #region Members
        private BusinessDbContext _context;
        private bool _showFiltersIsChecked;
        private ContentControl _filtersContentControl;
        public ICollection SalesHistories { get; set; }

        public bool ShowFiltersIsChecked
        {
            get
            {
                return _showFiltersIsChecked;
            }
            set
            {
                _showFiltersIsChecked = value;
                if (value)
                {
                    ShowFilters();
                }
                else
                {
                    FiltersContentControl = null;
                }
                OnPropertyChanged();    
            }
        }
        public ContentControl FiltersContentControl
        {
            get
            {
                return _filtersContentControl;
            }
            set
            {
                _filtersContentControl = value;
                OnPropertyChanged();
            }
        }

        public bool CanShowSalesHistory
        { get {return true; } }
        #endregion
        #region Constructors
        public ShowSalesHistoryMainViewModel(BusinessDbContext Context)
        {
            _context = Context;
            //SalesHistories = new ObservableCollection<SalesHistory>();
        }
        #endregion

        #region Commands
        public ICommand ShowSalesHistoryCommand { get { return new RelayCommand(ShowSalesHistory);} }
        public ICommand ShowFiltersContentControl { get { return new RelayCommand(ShowFilters);} }
        #endregion

        #region functions

        private void ShowFilters()
        {
            var obj = new ShowSalesHistoryFiltersUserControlViewModel(_context);
            FiltersContentControl = new FiltersUserControl {DataContext = obj};
        }
        private void ShowSalesHistory()
        {
            //if check box is unchecked
            //OnPropertyChanged("SalesHistories");
            
            Action GetSalesHistoriesNoFiltersAction = () =>
            {
                SalesHistories = (
                    from obj in _context.ShopSStorageDbContext.SalesHistories
                    select new
                    {
                        CathegoryName = obj.Product.Cathegory.CathegoryName,
                        ProductName = obj.Product.ProductName,
                        TotalSoldAmount =
                            _context.ShopSStorageDbContext.SalesHistories.Where(o => o.Product == obj.Product)
                                .Sum(o => o.SoldAmount)
                    }).Distinct().ToList();
                OnPropertyChanged("SalesHistories");
            };

            /*Thread nThread = new Thread(new ThreadStart(GetSalesHistoriesNoFiltersAction));
            nThread.IsBackground = true;
            nThread.Start();*/
            GetSalesHistoriesNoFiltersAction();
            //var SalesHistories2 = _context.ShopSStorageDbContext.SalesHistories.ToList<SalesHistory>();
            //OnPropertyChanged("SalesHistories");

        }
        #endregion
    }
}
