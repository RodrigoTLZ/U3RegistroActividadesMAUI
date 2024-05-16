using APIActividadesMAUI.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace APIActividadesMAUI.Services
{
    public class LoginService
    {
        HttpClient cliente = new()
        {
            BaseAddress = new Uri("https://actividadesteam7.websitos256.com/")
        };


        public async Task<string> Login (LoginDTO dto)
        {
            var dato = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await cliente.PostAsync($"api/login", dato);
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsStringAsync().Result;
            }
            else
            {
                return null;//implementar error
            }
        }

        public async Task<bool> Validar(string token)
        {
            var response = await cliente.GetAsync($"api/login/{token}");
            if(response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
