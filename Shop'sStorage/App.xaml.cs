namespace ShopSStorage
{
    using System.Windows;
    using ShopSStorage.ViewModels;
    using ShopSStorage.Views;
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var window = new MainWindow
            {
                DataContext = new MainViewModel()
            };

            window.ShowDialog();
        }
    }
}
