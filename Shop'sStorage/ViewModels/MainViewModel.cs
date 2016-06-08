using System.Net.NetworkInformation;
using System.Security.Cryptography;

namespace ShopSStorage.ViewModels
{
    using ShopSStorage.Models;
    using ShopSStorage.Schemats;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    public class MainViewModel : ViewModel
    {
        private readonly BusinessDbContext _context;
        public ICollection<Cathegory> Cathegories { get; private set; }
        private Cathegory _selectedCathegory;
        public ICollection<Product> Products { get; private set; }

        public MainViewModel() : this(new BusinessDbContext())
        {
        }

        public MainViewModel(BusinessDbContext context)
        {
            Cathegories = new ObservableCollection<Cathegory>();
            Products = new ObservableCollection<Product>();
            _context = context;
        }

        public Cathegory SelectedCathegory
        {
            get { return _selectedCathegory; }
            set
            {
                _selectedCathegory = value;
                OnPropertyChanged("IsSelected");
                if (IsSelected)
                {
                    GetProductsList();
                }
            }
        }

        public bool IsSelected
        {
            get { return _selectedCathegory != null; }
        }

        public bool CanBeDeleted
        {
            // TODO: Can be deleted only when the cathegory has no assotiaton with any product
            get { return false; }
        }
        public void GetProductsList()
        {
            Products.Clear();
            foreach (var p in _context.GetProducts(SelectedCathegory))
                Products.Add(p);
        }

        /// <summary>
        /// Commands
        /// </summary>
        public ICommand AddNewProductCommand
        {
            get
            {
                return new ActionCommand(anpc => AddNewProduct());
            }
        }
        public ICommand AddNewCathegoryCommand
        {
            get
            {
                return new ActionCommand(ancc => AddNewCathegory());
            }
        }
        public ICommand DeleteSelectedCathegoryCommand
        {
            get
            {
                return new ActionCommand(dsc => DeleteSelectedCathegory());
            }
        }
        public ICommand GetCathegoriesListCommand
        {
            get { return new ActionCommand(c => GetCathegoriesList()); }
        }

        /// <summary>
        /// Command's Methods
        /// </summary>
        private void AddNewProduct()
        {
            var prod = new Product()
            {
                ProductName = "New Product",
                StorageAmount = 0,
                Cathegory = SelectedCathegory
            };
            _context.AddNewProduct(prod);
            Products.Add(prod);
        }
        private void AddNewCathegory()
        {
            var cat = new Cathegory()
            {
                CathegoryName = "new Cathegory"
            };

        _context.AddNewCathegory(cat);
            Cathegories.Add(cat);
            SelectedCathegory = null;
        }
        private void DeleteSelectedCathegory()
        {
            _context.DeleteCathegory(SelectedCathegory);
            Cathegories.Remove(SelectedCathegory);
            SelectedCathegory = null;
        }
        private void GetCathegoriesList()
        {
            Cathegories.Clear();
            foreach (var c in _context.GetCathegories())
                Cathegories.Add(c);
        }
    }
}
