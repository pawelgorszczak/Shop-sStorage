using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MvvmSchemats;
using ShopSStorage.Models;

namespace ShopSStorage.ViewModels
{
    public class CathegoryViewModel : ViewModel, IDataErrorInfo
    {
        #region Members

        private BusinessDbContext _context;
        private Cathegory _cathegory;

        public string CathegoryName
        {
            get { return _cathegory.CathegoryName; }
            set
            {
                _cathegory.CathegoryName = value;
                OnPropertyChanged("IsValid");
            }
        }
        public bool IsValid { get { return !string.IsNullOrWhiteSpace(CathegoryName); } }
        #endregion

        #region Constructors

        public CathegoryViewModel() : this(new BusinessDbContext())
        {
            _cathegory = new Cathegory();
        }

        public CathegoryViewModel(Cathegory cathegory) : this(new BusinessDbContext())
        {
            _cathegory = new Cathegory();
            _cathegory = cathegory;
        }

        public CathegoryViewModel(BusinessDbContext context)
        {
            _context = context;
            this.AddNewCathegoryCommand = new RelayCommand<Window>(this.AddNewCathegory);
        }

        #endregion

        #region Commands and Commands' Fucntions

        public RelayCommand<Window> AddNewCathegoryCommand { get; private set; }
    
        public void AddNewCathegory(Window window)
        {
            _context.AddNewCathegory(_cathegory);
            window.Close();
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
                if (columnName == nameof(CathegoryName) && string.IsNullOrWhiteSpace(CathegoryName))
                    return "Cathegory name cannot be null";
                return string.Empty;
            }
        }

        #endregion
    }
}