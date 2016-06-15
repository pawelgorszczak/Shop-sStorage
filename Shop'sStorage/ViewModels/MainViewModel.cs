namespace ShopSStorage.ViewModels
{
    using ShopSStorage.Models;
    using ShopSStorage.Schemats;
    using ShopSStorage.Views;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    public class MainViewModel : ViewModel
    {
        private readonly BusinessDbContext _context;
        public ICollection<Cathegory> Cathegories { get; private set; }
        private Cathegory _selectedCathegory;
        private Product _selectedProduct;
        public ICollection<Product> Products { get; private set; }

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

        public Product SelectedProduct
        {
            get { return _selectedProduct;}
            set
            {
                _selectedProduct = value;
                OnPropertyChanged("ProductIsSelected");
            }
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
        public bool ProductIsSelected
        {
            get { return _selectedProduct != null; }
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
            if (IsSelected)
            {
                Products.Clear();
                foreach (var p in _context.GetProducts(SelectedCathegory))
                    Products.Add(p);
            }
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
        public  ICommand EditSelectedProductCommand { get { return new ActionCommand(espc => EditSelectedProduct()); } }

        /// <summary>
        /// Command's Methods
        /// </summary>
        private void EditSelectedProduct()
        {
            NewProductWindow viewNewProductWindow = new NewProductWindow { DataContext = new ProductViewModel(SelectedProduct) };
            viewNewProductWindow.ShowDialog();
        }
        private void AddNewProduct()
        {
            NewProductWindow viewNewProductWindow = new NewProductWindow {DataContext = new ProductViewModel()};
            viewNewProductWindow.ShowDialog();
            GetProductsList();
            OnPropertyChanged("Products");
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
            if (SelectedCathegory != null)
            {
                Cathegories.Remove(SelectedCathegory);
                SelectedCathegory = null;
            }
            
        }
        private void GetCathegoriesList()
        {
            Cathegories.Clear();
            foreach (var c in _context.GetCathegories())
                Cathegories.Add(c);
        }
    }
}
