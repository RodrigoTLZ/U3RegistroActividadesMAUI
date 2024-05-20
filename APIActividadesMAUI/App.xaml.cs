using APIActividadesMAUI.Services;
using APIActividadesMAUI.Views;

namespace APIActividadesMAUI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

        
    }
}
