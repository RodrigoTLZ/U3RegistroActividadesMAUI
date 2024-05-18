using APIActividadesMAUI.Models.DTOs;
using APIActividadesMAUI.Models.Entities;
using APIActividadesMAUI.Models.Validators;
using APIActividadesMAUI.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIActividadesMAUI.ViewModels
{
    public partial class ActividadesViewModel:ObservableObject
    {
        Repositories.RepositoryGeneric<Actividad> repositoryActividades = new();
        public ObservableCollection<Actividad> ListadoActividades { get; set; } = new();
        ActividadesService service = new();
        LoginService loginService = new();
        ActividadValidator validador = new();

        public Actividad ActividadSeleccionada { get; set; }

        public ActividadesViewModel()
        {
            ActualizarActividades();
            App.ActividadService.ActualizarDatos += ActividadService_ActualizarDatos;
        }

        private void ActividadService_ActualizarDatos()
        {
            ActualizarActividades();
        }

        [ObservableProperty]
        private ActividadDTO? actividad;

        [ObservableProperty]
        private string error = "";


        [RelayCommand]
        public void VerEditarActividad(int id)
        {
            ActividadSeleccionada = repositoryActividades.Get(id);
            if(ActividadSeleccionada != null)
            {
                Error = "";
                Shell.Current.GoToAsync("//EditarActividad");
            }
        }

        [RelayCommand]
        public void Nuevo()
        {
            Actividad = new();
            Shell.Current.GoToAsync("//AgregarActividad");
        }

        [RelayCommand]
        public void Cancelar()
        {
            Actividad = null;
            Error = "";
            Shell.Current.GoToAsync("//ListadoActividades");
        }

        public async Task Agregar()
        {
            try
            {
                if(Actividad != null)
                {
                    var resultado = validador.Validate(actividad);
                    if (resultado.IsValid)
                    {
                        await service.Agregar(actividad);
                        ActualizarActividades();
                        Cancelar();
                    }
                    else
                    {
                        Error = string.Join("\n", resultado.Errors.Select(x => x.ErrorMessage));
                    }
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
        }

        [RelayCommand]
        public async Task Editar()
        {
            try
            {
                if (ActividadSeleccionada != null)
                {
                    ActividadEditarValidator validator = new();
                    var actividadvalidada = validator.Validate(ActividadSeleccionada);
                    if (actividadvalidada.IsValid)
                    {
                        var departamento = loginService.ObtenerIdDepartamento();
                        var actividad = new ActividadDTO()
                        {
                            Titulo = ActividadSeleccionada.Titulo,
                            Descripcion = ActividadSeleccionada.Descripcion,
                            FechaRealizacion = ActividadSeleccionada.FechaRealizacion,
                            Estado = ActividadSeleccionada.Estado,
                            FechaActualizacion = ActividadSeleccionada.FechaActualizacion,
                            FechaCreacion = ActividadSeleccionada.FechaCreacion,
                            Id = ActividadSeleccionada.Id,
                            IdDepartamento = ActividadSeleccionada.IdDepartamento
                        };
                        await service.Editar(actividad);
                        Cancelar();
                    }
                    else
                    {
                        Error = string.Join("\n", actividadvalidada.Errors.Select(x => x.ErrorMessage));
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        void ActualizarActividades()
        {
            ListadoActividades.Clear();
            foreach (var item in repositoryActividades.GetAll())
            {
                ListadoActividades.Add(item);
            }
        }
    }
}
