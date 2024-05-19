using APIActividadesMAUI.Services;
using APIActividadesMAUI.Views;

namespace APIActividadesMAUI
{
    public partial class App : Application
    {
        public static LoginService LoginService { get; private set; }
        public static ActividadesService ActividadService { get; private set; }
        public App()
        {
            InitializeComponent();
            LoginService = new LoginService();
            ActividadService = new ActividadesService();
            LoginService.LoginExitoso += LoginService_LoginExitoso;
            MainPage = new AppShell();
        }

        private void LoginService_LoginExitoso()
        {
            Thread thread = new Thread(Sincronizador) { IsBackground = true };
            thread.Start();
            MainThread.BeginInvokeOnMainThread(() =>
            {
                MainPage = new NavigationPage(new ListadoActividadesView());
            });
        }

        async void Sincronizador()
        {
            int departamentoid = LoginService.GetDepartmentoId();

            if(departamentoid != 0)
            {
                while (true)
                {
                    await ActividadService.GetActividades(departamentoid);
                    Thread.Sleep(TimeSpan.FromSeconds(15));
                }
            }
            
        }
    }
}
