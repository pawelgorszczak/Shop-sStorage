using MvvmSchemats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ShopSStorage.ViewModels
{
    public class DeleteViewModel
    {
        private bool _x = false;
        public bool CanDelete { get {return _x;}}
        public DeleteViewModel()
        {
            this.NoCommand = new RelayCommand<Window>(this.DontAllowToDelete);
            this.YesCommand = new RelayCommand<Window>(this.AllowToDelete);
        }
        public RelayCommand<Window> NoCommand { get; private set; }
        public RelayCommand<Window> YesCommand { get; private set; }

        private void AllowToDelete(Window deleteWindow)
        {
            _x = true;
            deleteWindow.Close();
        }
        private void DontAllowToDelete(Window deleteWindow)
        {
            _x = false;
            deleteWindow.Close();
        }
    }
}
