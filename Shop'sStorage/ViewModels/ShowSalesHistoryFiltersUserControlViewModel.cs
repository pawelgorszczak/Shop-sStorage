using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MvvmSchemats;
using ShopSStorage.Models;

namespace ShopSStorage.ViewModels
{
    public class ShowSalesHistoryFiltersUserControlViewModel : ViewModel
    {
        #region Members
        private BusinessDbContext _context;
        private bool _isAllCheckBoxIsChecked;
        private DateTime? _dateFrom;
        private DateTime? _dateTo;
        public ObservableCollection<Cathegory> Cathegories { get; set; }

        public DateTime? DateFrom
        {
            set
            {
                _dateFrom = value;
                OnPropertyChanged();
            }
        }

        public DateTime? DateTo
        {
            set
            {
                _dateTo = value;
                OnPropertyChanged();
            }
        }
        public DateTime DateEnd { get { return DateTime.Now; } }
        public DateTime DateStart { get { return (_context.ShopSStorageDbContext.SalesHistories.Any())? _context.ShopSStorageDbContext.SalesHistories.Select(o => o.SoldDateTime).Min(): DateTime.Now; } }
        public bool IsAllCheckBoxIsChecked
        {
            get { return _isAllCheckBoxIsChecked; }
            set
            {
                _isAllCheckBoxIsChecked = value;
                if (value)
                {
                    foreach (var obj in Cathegories)
                    {
                        obj.IsSelected = true;
                    }
                }
                else
                {
                    foreach (var obj in Cathegories)
                    {
                        obj.IsSelected = false;
                    }
                }
                Cathegories.Clear();
                foreach (var obj in _context.GetCathegories())
                {
                    Cathegories.Add(obj);
                }
            }
        }
        #endregion

        #region Constructors
        public ShowSalesHistoryFiltersUserControlViewModel(BusinessDbContext context)
        {
            _context = context;
            Cathegories = new ObservableCollection<Cathegory>();
            Cathegories.Clear();
            foreach (var obj in _context.GetCathegories())
            {
                obj.IsSelected = false;
                Cathegories.Add(obj);
            }
        }
        #endregion

        #region Commands and funcionts
        public ICommand ShowSalesHistoryWithFiltersCommand
        {
            get { return new RelayCommand(ShowSalesHistoryWithFilters); }
        }

        private void ShowSalesHistoryWithFilters()
        {
            var From = _dateFrom==null? _context.ShopSStorageDbContext.SalesHistories.Select(o => o.SoldDateTime).Min() : _dateFrom.GetValueOrDefault() ;
            var To = _dateTo==null ? DateTime.Now : _dateTo.GetValueOrDefault();
            if (From > To)
            {
                var x = From;
                From = To;
                To = x;
            }
            var salesHistoryAllCathegories =
                    (from obj in _context.ShopSStorageDbContext.SalesHistories
                     where obj.SoldDateTime <= To && obj.SoldDateTime >= From
                     select new
                     {
                         CathegoryName = obj.Product.Cathegory.CathegoryName,
                         ProductName = obj.Product.ProductName,
                         TotalSoldAmount =
                            _context.ShopSStorageDbContext.SalesHistories.Where(o => o.Product == obj.Product)
                                .Sum(o => o.SoldAmount)
                     }).Distinct().ToList();
            if (Cathegories.Any(o => o.IsSelected == true) == true)
            {
                var selectedCathegories = Cathegories.Where(o => o.IsSelected == true).Select(o =>o.CathegoryName);
                var salesHistorySelectedCathegories =
                    salesHistoryAllCathegories.Where(o => selectedCathegories.Contains(o.CathegoryName)).ToList();
                salesHistoryAllCathegories = salesHistorySelectedCathegories;
            }
             
            OnLoadNewSalesHistoryData(salesHistoryAllCathegories);
        }
        #endregion

        #region Event

        public delegate void LoadNewSalesHistoyDataEventHandler(object source, EventArgs e);

        public event LoadNewSalesHistoyDataEventHandler LoadNewSalesHistoryData;

        public virtual void OnLoadNewSalesHistoryData(object x)
        {
            LoadNewSalesHistoryData?.Invoke(x, EventArgs.Empty);
        }

        #endregion
    }
}