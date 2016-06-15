using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using ShopSStorage.Models;
using ShopSStorage.Schemats;
using ShopSStorage.Views;

namespace ShopSStorage.ViewModels
{
    public class ProductViewModel : ViewModel
    {
        private readonly BusinessDbContext _context;
        public ICollection<Cathegory> Cathegories { get; private set; }
        public Product _product;
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
        public ProductViewModel() : this(new BusinessDbContext())
        {
            _product = new Product();
        }
        public ProductViewModel(Product product) : this(new BusinessDbContext())
        {
            _product = new Product();
            _product = product;
        }
        public ProductViewModel(BusinessDbContext context)
        {
            IsVisible = LstViewVisibility.Hidden.ToString();
            _context = context;
            Cathegories = new ObservableCollection<Cathegory>();
            foreach (var cat in _context.GetCathegories())
                Cathegories.Add(cat);
            OnPropertyChanged("Cathegories");
        }

        /// <summary>
        /// Commands
        /// </summary>
        public ICommand AddNewProductCommand { get { return new ActionCommand(anpc => AddNewProduct());} }
        public ICommand CanSelectCommand { get { return new ActionCommand(csc => MakeItVisible()); }  }

        private void AddNewProduct()
        {
            _context.AddNewProduct(Product);
        }
        private void MakeItVisible()
        {
            IsVisible = LstViewVisibility.Visible.ToString();
        }
    }
}
