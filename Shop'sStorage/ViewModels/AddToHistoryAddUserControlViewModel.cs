using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MvvmSchemats;
using ShopSStorage.Models;

namespace ShopSStorage.ViewModels
{
    public class AddToHistoryAddUserControlViewModel : ViewModel
    {
        #region Members

        private BusinessDbContext _context;
        private Cathegory _selectedCathegory;
        public ObservableCollection<Cathegory> Cathegories { get; private set; }
        public ObservableCollection<Product> Products { get; private set; }
        private ObservableCollection<SalesHistory> SalesHistories { get; set; }

        bool CathegoryIsSelected { get { return _selectedCathegory != null; } }
        

        public Cathegory SelectedCathegory
        {
            get { return _selectedCathegory; }
            set
            {
                _selectedCathegory = value;
                if (CathegoryIsSelected)
                {
                    Products.Clear();
                    foreach (var p in _context.GetProducts(_selectedCathegory))
                    {
                        p.IsSelected = false;
                        Products.Add(p);
                    }
                    OnPropertyChanged("Products");
                }
                OnPropertyChanged();
            }
        }

        #endregion


        #region Constructors
        public AddToHistoryAddUserControlViewModel(BusinessDbContext context, ObservableCollection<Cathegory> cathegories, ObservableCollection<SalesHistory> salesHistories)
        {
            _context = context;
            Cathegories = cathegories;
            SalesHistories = salesHistories;
            Products = new ObservableCollection<Product>();
        }
        #endregion

        #region Commands
        public ICommand AddCommand { get { return new RelayCommand(Add);} }
        #endregion

        #region Functions

        public delegate void ContentControlForceToCloseEventHandler(object source, EventArgs e);

        public event ContentControlForceToCloseEventHandler ContentControlForceToClose;

        protected virtual void OnContentControlForceToClose()
        {
            if (ContentControlForceToClose != null)
                ContentControlForceToClose(this, EventArgs.Empty);
        }

        private void Add()
        {
            foreach (var p in Products.Where(o =>o.IsSelected==true))
            {
                var s = new SalesHistory() {Product = p};
                if (SalesHistories.Count(o => o.Product == p) == 0)
                    SalesHistories.Add(s);
            }
            OnContentControlForceToClose();
        }
        #endregion
        
    }
}
