using APIActividadesMAUI.Models.DTOs;
using APIActividadesMAUI.Models.Entities;
using APIActividadesMAUI.Repositories;
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
        Repositories.RepositoryGeneric<Departamento> repository = new();


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
                await GetDepartamentos();
            }
            else
            {
                var errores = await response.Content.ReadAsStringAsync();
                throw new Exception(errores);
            }
        }


        public async Task EliminarDepartamento(DepartamentoDTO depa)
        {
            try
            {
                if(depa != null)
                {
                    var departamento = repository.Get(depa.Id);
                    repository.Delete(departamento);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task GetDepartamentos()
        {
            try
            {
                bool aviso = false;
                var response = await cliente.GetFromJsonAsync<List<DepartamentoDTO>>($"api/departamentos");
                if(response != null)
                {
                    foreach (DepartamentoDTO depa in response)
                    {
                        var entidad = repository.Get(depa.Id);

                        if (entidad == null)
                        {
                            entidad = new()
                            {
                                Id = depa.Id,
                                Nombre = depa.Nombre,
                                Password = depa.Password,
                                Username = depa.Username,
                                IdSuperior = depa.IdSuperior??0,
                            };
                            repository.Insert(entidad);
                            aviso=true;
                        }
                    }

                    if (aviso)
                    {
                        _ = MainThread.InvokeOnMainThreadAsync(() =>
                        {
                            ActualizarDatos?.Invoke();
                        });
                    }
                }

            }
            catch (Exception)
            {

            }
        }




    }
}
