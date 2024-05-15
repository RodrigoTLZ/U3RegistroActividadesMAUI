using APIActividadesMAUI.Views.ActividadesView;

namespace APIActividadesMAUI.Views;

public partial class LoginView : ContentPage
{
	public LoginView()
	{
		InitializeComponent();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ListadoActividadesView());
    }
}