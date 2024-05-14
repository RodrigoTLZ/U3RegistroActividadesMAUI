using APIActividadesMAUI.Views.ActividadesView;

namespace APIActividadesMAUI.Views;

public partial class ListadoActividadesView : ContentPage
{
	public ListadoActividadesView()
	{
		
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
		await Navigation.PushAsync(new AgregarActividadView());
    }
}