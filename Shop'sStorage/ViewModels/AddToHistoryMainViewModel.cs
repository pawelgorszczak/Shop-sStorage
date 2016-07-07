using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private ContentControl _contentControl;
        public ObservableCollection<Cathegory> Cathegories { get; private set; }
        public ObservableCollection<Product> Products { get; private set; }
        public ObservableCollection<Product> ChosenProducts { get; private set; }

        public ContentControl AddToHistoryMainWindowContentControl
        {
            get { return _contentControl;}
        }
        #endregion

        #region Constructors
        public AddToHistoryMainViewModel(BusinessDbContext context, ObservableCollection<Product> products, ObservableCollection<Cathegory> cathegories  )
        {
            Cathegories = cathegories;
            Products = products;
            _context = context;
            _contentControl = null;
        }
        #endregion

        #region Commands
        public ICommand AddNewCommand { get { return new RelayCommand(AddNew); } }
        public ICommand DeleteCommand { get { return new RelayCommand(Delete); } }
        #endregion

        #region Functions

        private void AddNew()
        {
            _contentControl = new AddUserControl();
            OnPropertyChanged("AddToHistoryMainWindowContentControl");
        }
        private void Delete()
        {
            _contentControl = null;
            OnPropertyChanged("AddToHistoryMainWindowContentControl");
        }
        #endregion
    }
}
