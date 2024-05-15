namespace APIActividadesMAUI.Views.ActividadesView;

public partial class AgregarActividadView : ContentPage
{
    public AgregarActividadView()
    {
        InitializeComponent();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        try
        {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Selecciona una imagen"
            });
            if (result != null)
            {
                var stream = await result.OpenReadAsync();
                selectedImage.Source = ImageSource.FromStream(() => stream);
            }
        }
        catch (Exception ex)
        {
            // Maneja posibles excepciones aquíawait DisplayAlert("Error", $"No se pudo seleccionar la imagen: {ex.Message}", "OK");             }
        }
    }
}