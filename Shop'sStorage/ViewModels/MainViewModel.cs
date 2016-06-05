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
            _context = context;
        }

        public Cathegory SelectedCathegory
        {
            get { return _selectedCathegory; }
            set
            {
                _selectedCathegory = value;
                OnPropertyChanged("IsSelected");
            }
        }

        public bool IsSelected
        {
            get { return _selectedCathegory != null; }
        }
        /// <summary>
        /// Commands
        /// </summary>
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
