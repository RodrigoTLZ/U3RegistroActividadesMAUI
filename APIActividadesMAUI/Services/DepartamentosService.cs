using APIActividadesMAUI.Models.DTOs;
using APIActividadesMAUI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace APIActividadesMAUI.Services
{
    public class DepartamentosService
    {
        HttpClient cliente;
        Repositories.RepositoryGeneric<Departamento> DepartamentosRepository = new();
        public event Action? ActualizarDatos;

        
        public DepartamentosService()
        {
            cliente = new()
            {
               // BaseAddress = new Uri("")   ACA VA LA API
            };
        }


        public async Task Agregar(DepartamentoDTO dto)
        {
            var response = await cliente.PostAsJsonAsync("api/departamentos", dto);

            if (response.IsSuccessStatusCode)
            {
                //
            }
            else
            {
                var errores = await response.Content.ReadAsStringAsync();
                throw new Exception(errores);
            }
        }

        public async Task GetDepartamentos()
        {
            try
            {
                var fecha = Preferences.Get("UltimaFechaActualizacion", DateTime.MinValue);
                bool aviso = false;
               // var response = await cliente.GetFromJsonAsync<List<Depart>>($"api/libros/{fecha:yyyy-MM-dd}/{fecha:HH}/{fecha:mm}");


            }
            catch (Exception)
            {

                throw;
            }
        }




    }
}
