using APIActividadesMAUI.Models.DTOs;
using APIActividadesMAUI.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIActividadesMAUI.ViewModels
{
    public partial class LoginViewModel:ObservableObject
    {
        LoginService loginService = new();
        ActividadesService actividadesService = new();

        [ObservableProperty]
        public string? username;

        [ObservableProperty]
        public string? password;

        [ObservableProperty]
        public string? errores;

        


        [RelayCommand]
        public async void Login()
        {
            

            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace((password)))
            {
                var tokensito = await loginService.Login(new LoginDTO
                {
                    Username = username,
                    Password = password
                });
                if (tokensito != null)
                {
                    await SecureStorage.SetAsync("tkn", tokensito);
                    int departamentoid = await loginService.GetDepartmentoId();
                    await actividadesService.GetActividades(departamentoid);

                    await Shell.Current.GoToAsync("//ListadoActividades");
                }
                else
                {
                    Errores = "Verifique haber ingresado el usuario o la contraseña incorrecta";
                }
            }
            else
            {
                Errores = "El usuario y la contraseña no pueden estar vacíos";
            }
        }


        async void ValidarToken()
        {
            var tokenazo = await SecureStorage.GetAsync("tokenazo");
            if(tokenazo != null)
            {
                var tokenvalido = await loginService.Validar(tokenazo);
                if (tokenvalido)
                {
                    await Shell.Current.GoToAsync("//ListaActividades");
                }
                else
                {
                    SecureStorage.RemoveAll();
                }
            }
            else
            {
                return;
            }
        }
    }
}
