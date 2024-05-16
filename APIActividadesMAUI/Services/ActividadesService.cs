using APIActividadesMAUI.Models.DTOs;
using APIActividadesMAUI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace APIActividadesMAUI.Services
{
    public class ActividadesService
    {
        HttpClient cliente;
        Repositories.RepositoryGeneric<ActividadDTO> repository = new();
        public event Action? ActualizarDatos;

        public ActividadesService()
        {
            cliente = new()
            {
                BaseAddress = new Uri("https://actividadesteam7.websitos256.com/")
            };
        }

        public async Task Agregar(ActividadDTO dto)
        {
            var response = await cliente.PostAsJsonAsync("api/actividades", dto);

            if(response.IsSuccessStatusCode)
            {
                await GetActividades();
            }
            else
            {
                var errores = await response.Content.ReadAsStringAsync();
                throw new Exception(errores);
            }
        }

        public async Task Eliminar(int id)
        {
            var response = await cliente.DeleteAsync("api/actividades/" + id);

            if (response.IsSuccessStatusCode)
            {
                await GetActividades();
            }
        }

        public async Task Editar(ActividadDTO dto)
        {
            var response = await cliente.PutAsJsonAsync("api/actividades", dto);

        }


        public async Task GetActividades()
        {
            try
            {
                var fecha = Preferences.Get("UltimaFechaActualizacion", DateTime.MinValue);
                bool aviso = false;
                var response = await cliente.GetFromJsonAsync<List<ActividadDTO>>($"api/actividades/{fecha:yyyy-MM-dd}/{fecha:HH}/{fecha:mm}");

                if (response != null)
                {
                    foreach (ActividadDTO actividad in response)
                    {
                        var entidad = repository.Get(actividad.Id);

                        if (entidad == null && actividad.Estado == 1)
                        {
                            entidad = new()
                            {
                                Id = actividad.Id,
                                Descripcion = actividad.Descripcion,
                                Estado = actividad.Estado,
                                FechaRealizacion = actividad.FechaRealizacion,
                                FechaActualizacion = actividad.FechaActualizacion,
                                FechaCreacion = actividad.FechaCreacion,
                                IdDepartamento = actividad.IdDepartamento,
                                Titulo = actividad.Titulo
                            };
                            repository.Insert(entidad);
                            aviso = true;
                        }
                        else
                        {
                            if(entidad != null)
                            {
                                if(actividad.Estado == 2)
                                {
                                    repository.Delete(entidad);
                                    aviso = true;
                                }
                                else
                                {
                                    if(actividad.Titulo != entidad.Titulo ||
                                        actividad.Estado != entidad.Estado ||
                                        actividad.FechaActualizacion != entidad.FechaActualizacion ||
                                        actividad.FechaRealizacion != entidad.FechaRealizacion ||
                                        actividad.Descripcion != entidad.Descripcion)
                                    {
                                        repository.Update(entidad);
                                        aviso = true;
                                    }
                                }
                            }
                        }
                    }

                    if (aviso)
                    {
                        _ = MainThread.InvokeOnMainThreadAsync(() =>
                        {
                            ActualizarDatos?.Invoke();
                        });
                    }
                    Preferences.Set("UltimaFechaActualizacion", response.Max(x => x.FechaCreacion));
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
