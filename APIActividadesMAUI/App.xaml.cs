using APIActividadesMAUI.Services;

namespace APIActividadesMAUI
{
    public partial class App : Application
    {
        public static ActividadesService ActividadService { get; set; } = new();   
        public App()
        {
            InitializeComponent();

            Thread thread = new Thread(Sincronizador) { IsBackground = true };
            thread.Start();
            MainPage = new AppShell();
        }

        async void Sincronizador()
        {
            while (true)
            {
                await ActividadService.GetActividades();
                Thread.Sleep(TimeSpan.FromSeconds(15));
            }
        }
    }
}
