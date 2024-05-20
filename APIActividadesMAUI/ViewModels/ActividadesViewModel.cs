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
        ActividadesService service { get; set; } = new();
        LoginService loginService { get; set; } = new();
        ActividadValidator validador = new();

        public Actividad ActividadSeleccionada { get; set; }

        public ActividadesViewModel()
        {
            ActualizarActividades();
            loginService.LoginExitoso += LoginService_LoginExitoso;
            service.ActualizarDatos += Service_ActualizarDatos;
        }

        private void Service_ActualizarDatos()
        {
            ActualizarActividades();
        }

        private void LoginService_LoginExitoso()
        {
            Thread thread = new Thread(Sincronizador) { IsBackground = true };
            thread.Start();
        }

        [ObservableProperty]
        public ActividadDTO? actividad;

        [ObservableProperty]
        public string error = "";


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
        public void VerEliminarActividad(int id)
        {
            ActividadSeleccionada = repositoryActividades.Get(id);
            if (ActividadSeleccionada != null)
            {
                Shell.Current.GoToAsync("//EliminarActividad");
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

        [RelayCommand]
        public async Task Agregar()
        {
            try
            {
                actividad.IdDepartamento = await loginService.GetDepartmentoId();
                if(Actividad != null)
                {
                    var resultado = validador.Validate(actividad);
                    if (resultado.IsValid)
                    {
                        actividad.FechaRealizacion = actividad.FechaRealizacion;
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
        public async Task Eliminar()
        {
            try
            {
                if(ActividadSeleccionada != null)
                {
                    await service.Eliminar(ActividadSeleccionada.Id);
                    ActividadSeleccionada = null;
                    Cancelar();
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
            catch (Exception ex)
            {
                Error = ex.Message;
            }
        }

        async void Sincronizador()
        {
            int departamentoid = await loginService.GetDepartmentoId();

            if (departamentoid != 0)
            {
                while (true)
                {
                    await service.GetActividades(departamentoid);
                    await Task.Delay(TimeSpan.FromSeconds(15));
                }
            }

        }


        void ActualizarActividades()
        {
            ListadoActividades.Clear();
            foreach (var item in repositoryActividades.GetAll().OrderBy(x=>x.FechaCreacion))
            {
                ListadoActividades.Add(item);
            }
        }
    }
}
