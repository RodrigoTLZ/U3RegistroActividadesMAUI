using Android.Telephony;
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
    public partial class DepartamentosViewModel:ObservableObject
    {
        Repositories.RepositoryGeneric<Departamento> depaRepository = new();

        public ObservableCollection<Departamento> ListadoDepartamentos = new();

        DepartamentosService service = new();
        DepartamentoValidator validador = new();

        public Departamento DepartamentoSeleccionado { get; set; }

        public DepartamentosViewModel()
        {
            ActualizarDepartamentos();
           
        }

        [ObservableProperty]
        private DepartamentoDTO departamento;

        [ObservableProperty]
        private string error = "";

        [RelayCommand]
        public void Nuevo()
        {
            Departamento = new();
            Shell.Current.GoToAsync("//AgregarDepartamento");
        }


        [RelayCommand]
        public void VerDepartamentos()
        {
            ActualizarDepartamentos();
            Shell.Current.GoToAsync("//ListadoActividadesAdmin");
        }

        [RelayCommand]
        public void VerEliminarDepartamento(int id)
        {

            DepartamentoSeleccionado = depaRepository.Get(id);
            if(DepartamentoSeleccionado != null)
            {
                Shell.Current.GoToAsync("//EliminarDepartamento");
            }
        }


        [RelayCommand]
        public void VerEditarDepartamento(int id)
        {
            DepartamentoSeleccionado = depaRepository.Get(id);
            if (DepartamentoSeleccionado != null)
            {
                Error = "";
                Shell.Current.GoToAsync("//EditarDepartamento");
            }
        }


        [RelayCommand]
        public async Task Editar()
        {
            try
            {
                if(DepartamentoSeleccionado != null)
                {
                    DepartamentoValidator validator = new();
                    var departamentovalidado = validator.Validate(DepartamentoSeleccionado);
                    if(departamentovalidado.IsValid)
                    {
                        var depa = new DepartamentoDTO()
                        {
                            Id = DepartamentoSeleccionado.Id,
                            IdSuperior = DepartamentoSeleccionado.IdSuperior,
                            Nombre = DepartamentoSeleccionado.Nombre,
                            Password = DepartamentoSeleccionado.Password,
                            Username = DepartamentoSeleccionado.Username
                        };
                        await service.EditarDepartamento(depa);
                        Cancelar();
                    }
                    else
                    {
                        Error = string.Join("\n", departamentovalidado.Errors.Select(x => x.ErrorMessage));

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
                if(Departamento!= null)
                {
                    await service.EliminarDepartamento(Departamento.Id);
                    Cancelar();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        [RelayCommand]
        public void Cancelar()
        {
            Departamento = null;
            Error = "";
            Shell.Current.GoToAsync("//ListadoActividadesAdmin");
        }


        [RelayCommand]
        public async Task Agregar()
        {
            try
            {
                if(Departamento != null)
                {
                    var resultado = validador.Validate(Departamento);
                    if(resultado.IsValid)
                    {
                        await service.Agregar(Departamento);
                        ActualizarDepartamentos();
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



        void ActualizarDepartamentos()
        {
            ListadoDepartamentos.Clear();
            foreach (var item in depaRepository.GetAll())
            {
                ListadoDepartamentos.Add(item);
            }
        }
    }
}
