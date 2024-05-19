using APIActividadesMAUI.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Security.Principal;
using CommunityToolkit.Mvvm.ComponentModel;
using System.IdentityModel.Tokens.Jwt;

namespace APIActividadesMAUI.Services
{
    public class LoginService
    {
        public event Action? LoginExitoso;

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
                LoginExitoso?.Invoke();
                return response.Content.ReadAsStringAsync().Result;
            }
            else
            {
                return null;//implementar error
            }
        }
        public string GetToken()
        {
            string token = SecureStorage.GetAsync("tkn").Result;
            return token;
        }



        public int GetDepartmentoId()
        {
            var token = GetToken();
            // Inicializa el manejador del token
            var handler = new JwtSecurityTokenHandler();

            // Si el token está en un formato JWT, puedes decodificarlo así
            if (handler.CanReadToken(token))
            {
                var jwtToken = handler.ReadJwtToken(token);

                // Obtén la claim del DepartamentoId
                var departmentClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "DepartamentoId");

                if (departmentClaim != null)
                {
                    return int.Parse(departmentClaim.Value);
                }
            }
            return 0;
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
