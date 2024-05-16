using APIActividadesMAUI.Views.ActividadesView;
using APIActividadesMAUI.Views.DepartamentosView;

namespace APIActividadesMAUI.Views;

public partial class ListadoActividadesView : ContentPage
{
	public ListadoActividadesView()
	{
		InitializeComponent();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
		await Navigation.PushAsync(new AgregarDepartamentoView());
    }

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new EditarDepartamento());
    }

    private async void Button_Clicked_2(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new EliminarActividadView());
    }
}