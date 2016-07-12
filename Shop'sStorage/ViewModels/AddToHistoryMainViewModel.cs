using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MvvmSchemats;
using ShopSStorage.Models;
using ShopSStorage.Views.AddToHistory;

namespace ShopSStorage.ViewModels
{
    public class AddToHistoryMainViewModel : ViewModel
    {
        #region Members
        private BusinessDbContext _context;
        private SalesHistory _selectedSalesHistory;
        private ContentControl _contentControl;
        public ObservableCollection<Cathegory> Cathegories { get; private set; }
        public ObservableCollection<Product> Products { get; private set; }
        public ObservableCollection<SalesHistory> SalesHistories { get; private set; } = new ObservableCollection<SalesHistory>();

        public ContentControl AddToHistoryMainWindowContentControl
        {
            get { return _contentControl;}
        }
        public bool IsValid { get { return true; } }
        public bool IsSelected { get { return _selectedSalesHistory != null; } }

        public SalesHistory SelectedSalesHistory
        {
            get { return _selectedSalesHistory; }
            set
            {
                _selectedSalesHistory = value;
                OnPropertyChanged("IsSelected");
            }
        }
        #endregion

        #region Constructors
        public AddToHistoryMainViewModel(BusinessDbContext context, ObservableCollection<Product> products, ObservableCollection<Cathegory> cathegories  )
        {
            Cathegories = cathegories;
            Products = products;
            _context = context;
            _contentControl = null;
            this.SaveToHistoryCommand = new RelayCommand<Window>(this.SaveToHistory);
        }
        #endregion

        #region Commands
        public RelayCommand<Window> SaveToHistoryCommand { get; private set; }
        public ICommand AddNewCommand { get { return new RelayCommand(AddNew); } }
        public ICommand DeleteSelectedSalesHistoryCommand { get { return new RelayCommand(DeleteSelectedSalesHistory); } }
        #endregion

        #region Functions

        private void SaveToHistory(Window window)
        {
            if (SalesHistories.Any())
            {
                var temp = DateTime.Now;
                foreach (var obj in SalesHistories)
                {
                    obj.SoldDateTime = temp;
                }
                _context.AddSalesHistories(SalesHistories);
                foreach (var obj in SalesHistories)
                {
                    _context.ChangeProductStorageAmount(obj.Product, obj.SoldAmount);
                }
                window.Close();
                OnProductsAddedToHIstory();
            }
        }
        private void DeleteSelectedSalesHistory()
        {
            if (_selectedSalesHistory != null)
            {
                SalesHistories.Remove(_selectedSalesHistory);
            }
        }
        private void AddNew()
        {
            var addToHistoryAddUserControlViewModel = new AddToHistoryAddUserControlViewModel(_context, Cathegories,
                SalesHistories);
            addToHistoryAddUserControlViewModel.ContentControlForceToClose += this.OnContentControlForceToClose;
            _contentControl = new AddUserControl {DataContext = addToHistoryAddUserControlViewModel};
            OnPropertyChanged("AddToHistoryMainWindowContentControl");
        }
        #endregion

        #region CloseControl Event

        //event
        public void OnContentControlForceToClose(object source, EventArgs e)
        {
            _contentControl = null;
            OnPropertyChanged("AddToHistoryMainWindowContentControl");
        }

        #endregion

        #region ProductsAddedToHIstory Event
        public delegate void ProductsAddedToHIstoryEventHandler(object source, EventArgs e);

        public event ProductsAddedToHIstoryEventHandler ProductsAddedToHIstory;

        public virtual void OnProductsAddedToHIstory()
        {
            if(ProductsAddedToHIstory!=null)
                this.ProductsAddedToHIstory(this,EventArgs.Empty);
        }
        #endregion
    }
}
