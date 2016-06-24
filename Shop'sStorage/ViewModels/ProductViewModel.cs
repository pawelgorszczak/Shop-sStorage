using System.Diagnostics;
using System.Windows.Controls;

namespace ShopSStorage.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq.Expressions;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Input;
    using MvvmSchemats;
    using ShopSStorage.Models;
    using ShopSStorage.Views;

    public class ProductViewModel : ViewModel, IDataErrorInfo
    {
#region Members
        private readonly BusinessDbContext _context;
        public ICollection<Cathegory> Cathegories { get; private set; }
        private Product _product;
        private string _test;
#region Product Members

        public string ProductName
        {
            get { return _product.ProductName; }
            set
            {
                _product.ProductName = value;
                //OnPropertyChanged("ProductName");
                OnPropertyChanged("NewProductIsValid");
            }
        }
        public int StorageAmount
        {
            get { return _product.StorageAmount; }
            set
            {
                _product.StorageAmount = value;
                OnPropertyChanged("StorageAmount");
                OnPropertyChanged("NewProductIsValid");
            }
        }
        public decimal DetailPrice
        {
            get { return _product.DetailPrice; }
            set
            {
                _product.DetailPrice = value;
                OnPropertyChanged("DetailPrice");
                OnPropertyChanged("NewProductIsValid");
            }
        }
        public  Cathegory Cathegory
        {
            get { return _product.Cathegory; }
            set
            {
                _product.Cathegory = value;
                OnPropertyChanged("CathegoryName");
                OnPropertyChanged("NewProductIsValid");
            }
        }

        public string CathegoryName
        {
            get {
                if (_product.Cathegory == null)
                {
                    return string.Empty;
                }
                return _product.Cathegory.CathegoryName;
            }
        }
#endregion

        private enum LstViewVisibility
        {
            Hidden,
            Visible
        }
        private string _isVisible;
        public string Test
        {
            get { return _test; }
            set { _test = value; }
        }
        public string IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;
                OnPropertyChanged("IsVisible");
            }
        }

        public bool NewProductIsValid
        {
            get
            {
                if (string.IsNullOrEmpty(_product.ProductName) ||
                    string.IsNullOrWhiteSpace(_product.StorageAmount.ToString()) ||
                    string.IsNullOrWhiteSpace(_product.DetailPrice.ToString()) ||
                    _product.Cathegory == null)    
                    return false;
                return true;
            }
        }
#endregion

#region Constructors
        public ProductViewModel() : this(new BusinessDbContext())
        {
            _product = new Product();
        }
        public ProductViewModel(Product product) : this(new BusinessDbContext())
        {
            //_product = new Product();
            _product = product;
        }

        public ProductViewModel(BusinessDbContext context, Product product)
        {
            _product = product;
            IsVisible = LstViewVisibility.Hidden.ToString();
            _context = context;
            Cathegories = new ObservableCollection<Cathegory>();
            foreach (var cat in _context.GetCathegories())
                Cathegories.Add(cat);
            OnPropertyChanged("Cathegories");
            this.AddNewProductCommand = new RelayCommand<Window>(this.AddNewProduct);

        }
        public ProductViewModel(BusinessDbContext context)
        {
            IsVisible = LstViewVisibility.Hidden.ToString();
            _context = context;
            Cathegories = new ObservableCollection<Cathegory>();
            foreach (var cat in _context.GetCathegories())
                Cathegories.Add(cat);
            OnPropertyChanged("Cathegories");
            this.AddNewProductCommand = new RelayCommand<Window>(this.AddNewProduct);
        }
        #endregion

#region Command and Cammand's Funcionts

        //public ICommand AddNewProductCommand { get { return new RelayCommand<Window>(AddNewProduct);} }
        public RelayCommand<Window> AddNewProductCommand { get; private set; }
        public ICommand CanSelectCommand { get { return new RelayCommand(MakeItVisible); }  }
        
        private void AddNewProduct(Window productWindow)
        {
            if (NewProductIsValid)
            {
                _context.AddNewProduct(_product);
                productWindow.Close();
            }
        }
        private void MakeItVisible()
        {
            IsVisible = LstViewVisibility.Visible.ToString();
        }
        #endregion
        #region IdataErrorInfo
        public string Error
        {
            get
            {
                return null;
            }
        }

        public string this[string columnName]
        {
            get
            {

                switch (columnName)
                {
                    case nameof(ProductName):
                        if (string.IsNullOrWhiteSpace(ProductName))
                            return "Product name can not be empty";
                        break;
                    case nameof(StorageAmount):
                        if (string.IsNullOrWhiteSpace(StorageAmount.ToString()))
                            return "Storage Amount can not be null";
                        break;
                    case nameof(DetailPrice):
                        if (string.IsNullOrWhiteSpace(DetailPrice.ToString()))
                            return "Detail price can not be null";
                        break;
                    case nameof(CathegoryName):
                        if (string.IsNullOrWhiteSpace(CathegoryName))
                            return "Cathegory can not be empty";
                        break;
                }
                return string.Empty;
            }
        }
        #endregion

    }
}
