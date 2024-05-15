﻿using APIActividadesMAUI.Models.DTOs;
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

        public void Borrar(int id)
        {
            var departamento = depaRepository.Get(id);
            if(departamento != null)
            {
                Departamento = departamento;
                Shell.Current.GoToAsync("//EliminarDepartamento");
            }
        }

        public void Modificar(int id)
        {
            var departamento = depaRepository.Get(id);
            if (departamento != null)
            {
                Departamento = departamento;
                Shell.Current.GoToAsync("//EditarDepartamento");
            }
        }


        [RelayCommand]
        public async Task Editar()
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
        }
        


        

        [RelayCommand]
        public async Task Eliminar()
        {
            try
            {
                if(Departamento!= null)
                {
                    await service.EliminarDepartamento(Departamento);
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
            Shell.Current.GoToAsync("//ListadoActividades");
        }

        DepartamentoValidator validador = new();

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
