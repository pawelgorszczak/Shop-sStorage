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

    public class ProductViewModel : ViewModel
    {
#region Members
        private readonly BusinessDbContext _context;
        public ICollection<Cathegory> Cathegories { get; private set; }
        private Product _product;
        private enum LstViewVisibility
        {
            Hidden,
            Visible
        }
        private string _isVisible;

        public Product Product
        {
            get { return _product;}
            set
            {
                _product = value;
                OnPropertyChanged("Product");
            }
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
            _context.AddNewProduct(Product);
            productWindow.Close();
        }
        private void MakeItVisible()
        {
            IsVisible = LstViewVisibility.Visible.ToString();
        }
        #endregion
        
    }
}
