using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MvvmSchemats;
using ShopSStorage.Models;

namespace ShopSStorage.ViewModels
{
    public class CathegoryViewModel : ViewModel
    {
        #region Members

        private BusinessDbContext _context;
        private Cathegory _cathegory;

        public Cathegory Cathegory
        {
            get { return _cathegory; }
            set { _cathegory = value; }
        }

        #endregion

        #region Constructors

        public CathegoryViewModel() : this(new BusinessDbContext())
        {
            _cathegory = new Cathegory();
        }
        public CathegoryViewModel(Cathegory cathegory) : this(new BusinessDbContext())
        {
            _cathegory = new Cathegory();
            Cathegory = cathegory;
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
    }
}