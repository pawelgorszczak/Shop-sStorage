using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using ShopSStorage.Views.AddToHistory;
using ShopSStorage.Views.ShowSalesHistory;

namespace ShopSStorage.ViewModels
{
    using ShopSStorage.Models;
    using MvvmSchemats;
    using ShopSStorage.Views;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    public class MainViewModel : ViewModel
    {
        #region Members
        private readonly BusinessDbContext _context;
        public ObservableCollection<Cathegory> Cathegories { get; private set; }
        private Cathegory _selectedCathegory;
        private Product _selectedProduct;
        public ObservableCollection<Product> Products { get; private set; }



        public Product SelectedProduct
        {
            get { return _selectedProduct;}
            set
            {
                if (_selectedProduct != value)
                {
                    _selectedProduct = value;
                    OnPropertyChanged("ProductIsSelected");
                }
            }
        }
        public Cathegory SelectedCathegory
        {
            get { return _selectedCathegory; }
            set
            {
                if (_selectedCathegory != value)
                {
                    _selectedCathegory = value;
                    OnPropertyChanged("IsSelected");
                    OnPropertyChanged("IsSelectedAndCanBeDeleted");
                    if (IsSelected)
                    {
                        GetProductsList();
                    }
                }
            }
        }
        public bool ProductIsSelected
        {
            get { return _selectedProduct != null; }
        }
        public bool IsSelected
        {
            get { return _selectedCathegory != null; }
        }

        public bool IsSelectedAndCanBeDeleted
        {
            get
            {
                
                if (IsSelected) 
                {
                    if (_context.GetProducts(_selectedCathegory).Count != 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        public void OnProductsHistoryAdded(object source, EventArgs e)
        {
            GetProductsList();
        }
        public void GetProductsList()
        {
            if (IsSelected)
            {
                Products.Clear();
                foreach (var p in _context.GetProducts(SelectedCathegory))
                    Products.Add(p);
                OnPropertyChanged("Products");
            }
        }
        #endregion

        #region Constructors
        public MainViewModel() : this(new BusinessDbContext())
        {
        }

        public MainViewModel(BusinessDbContext context)
        {
            Cathegories = new ObservableCollection<Cathegory>();
            Products = new ObservableCollection<Product>();
            _context = context;
            GetCathegoriesList();
        }
        #endregion

        #region Command and Command's Functions
        public ICommand EditSelectedCathegoryCommand { get { return new RelayCommand(EditSelectedCathegory);} }
        public ICommand DeleteSelectedProductCommand { get { return new RelayCommand(DeleteSelectedProduct);} }
        public ICommand AddNewProductCommand{get{return new RelayCommand(AddNewProduct);}}
        public ICommand AddNewCathegoryCommand{get{return new RelayCommand(AddNewCathegory);}}
        public ICommand DeleteSelectedCathegoryCommand{get{return new RelayCommand(DeleteSelectedCathegory);}}
        public ICommand GetCathegoriesListCommand{get { return new RelayCommand(GetCathegoriesList); }}
        public  ICommand EditSelectedProductCommand { get { return new RelayCommand(EditSelectedProduct); } }
        public ICommand AddToHistoryCommand { get { return new RelayCommand(AddToHistory); } }
        public ICommand ShowSalesHistoryWindowCommand { get { return new RelayCommand(ShowSalesHistoryWindow);} }

        /// <summary>
        /// Command's Methods
        /// </summary>
        public void ShowSalesHistoryWindow()
        {
            var obj = new ShowSalesHistoryMainViewModel(_context);
            ShowSalesHistoryMainWindow showSalesHistoryMainWindow = new ShowSalesHistoryMainWindow {DataContext = obj};
            showSalesHistoryMainWindow.ShowDialog();
        }
        private void AddToHistory()
        {
            var obj = new AddToHistoryMainViewModel(_context, Products, Cathegories);
            obj.ProductsAddedToHIstory += OnProductsHistoryAdded;
            AddToHistoryMainWindow historyMainViewModel = new AddToHistoryMainWindow {DataContext = obj};
            historyMainViewModel.ShowDialog();
        }
        private void EditSelectedCathegory()
        {
            CathegoryWindow cathegoryWindow = new CathegoryWindow {DataContext = new CathegoryViewModel(_selectedCathegory)};
            cathegoryWindow.ShowDialog();
            GetCathegoriesList();
        }
        private void DeleteSelectedProduct()
        {
            if (ProductIsSelected)
            {
                var deleteViewModel = new DeleteViewModel();
                DeleteWindow deleteWindow = new DeleteWindow {DataContext = deleteViewModel};
                deleteWindow.ShowDialog();
                if (deleteViewModel.CanDelete)
                {
                    _selectedProduct.Cathegory = _selectedCathegory;
                    _context.DeleteProduct(_selectedProduct);
                    Products.Remove(_selectedProduct);
                    OnPropertyChanged("IsSelectedAndCanBeDeleted");
                }

            }
        }
        private void EditSelectedProduct()
        {
            ProductWindow viewProductWindow = new ProductWindow { DataContext = new ProductViewModel(_context,SelectedProduct) };
            viewProductWindow.ShowDialog();
            GetProductsList();
            OnPropertyChanged("Products");
        }
        private void AddNewProduct()
        {
            if (IsSelected)
            {
                ProductWindow viewProductWindow = new ProductWindow { DataContext = new ProductViewModel(_context, new Product() { Cathegory = _selectedCathegory}) };
                viewProductWindow.ShowDialog();
            }
            else
            {
                ProductWindow viewProductWindow = new ProductWindow { DataContext = new ProductViewModel() };
                viewProductWindow.ShowDialog();
            }
            
            GetProductsList();
            OnPropertyChanged("Products");
        }
        private void AddNewCathegory()
        {
            CathegoryWindow cathegoryWindow = new CathegoryWindow { DataContext = new CathegoryViewModel() };
            cathegoryWindow.ShowDialog();
            GetCathegoriesList();
        }
        private void DeleteSelectedCathegory()
        {
            if (SelectedCathegory != null)
            {
                var deleteViewModel = new DeleteViewModel();
                DeleteWindow deleteWindow = new DeleteWindow { DataContext = deleteViewModel };
                deleteWindow.ShowDialog();
                if (deleteViewModel.CanDelete)
                {
                    _context.DeleteCathegory(SelectedCathegory);
                    Cathegories.Remove(SelectedCathegory);
                    SelectedCathegory = null;
                    Products.Clear();
                }
                
            }
        }

        private void GetCathegoriesList()
        {
            Cathegories.Clear();
            foreach (var c in _context.GetCathegories())
                Cathegories.Add(c);
        }
        #endregion


    }
}
