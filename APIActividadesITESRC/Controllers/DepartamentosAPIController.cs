using APIActividadesITESRC.Models.DTOs;
using APIActividadesITESRC.Models.Entities;
using APIActividadesITESRC.Models.Validators;
using APIActividadesITESRC.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace APIActividadesITESRC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartamentosAPIController : ControllerBase
    {
        public DepartamentosRepository Repository { get; }

        public DepartamentosAPIController(DepartamentosRepository repository)
        {
            Repository = repository;       
        }


        [HttpPost]

        public IActionResult Post(DepartamentoDTO dto)
        {
            DepartamentoValidator validator = new();
            var resultados = validator.Validate(dto);

            if (resultados.IsValid)
            {
                Departamentos entity = new()
                {
                    Id = 0,
                    Actividades = dto.Actividades,
                    IdSuperior = dto.IdSuperior,
                    IdSuperiorNavigation = dto.IdSuperiorNavigation,
                    InverseIdSuperiorNavigation = dto.InverseIdSuperiorNavigation,
                    Nombre = dto.Nombre,
                    Password = dto.Password,
                    Username = dto.Username
                };
                Repository.Insert(entity);
                return Ok();
            }
            else
            {
                return BadRequest(resultados.Errors.Select(x => x.ErrorMessage));
            }

        }


        [HttpGet]
        public IActionResult Get()
        {
            var departamentos = Repository.GetAll().Select(x=> new DepartamentoDTO
            {
                Nombre = x.Nombre,
                Id = x.Id,
                Actividades= x.Actividades,
                IdSuperior= x.IdSuperior,
                IdSuperiorNavigation = x.IdSuperiorNavigation,
                InverseIdSuperiorNavigation= x.InverseIdSuperiorNavigation,
                Password = x.Password,
                Username = x.Username
            });

            return Ok(departamentos);    
        }


        [HttpPut("{Id}")]
        public IActionResult Put(DepartamentoDTO dto)
        {
            DepartamentoValidator validator = new();
            var results = validator.Validate(dto);
            if (results.IsValid)
            {
                var entidadDepartamento = Repository.GetById(dto.Id);
                if (entidadDepartamento == null)
                {
                    return NotFound();
                }
                else
                {
                    entidadDepartamento.Nombre = dto.Nombre;
                    entidadDepartamento.Username = dto.Username;
                    entidadDepartamento.Password = dto.Password;
                    entidadDepartamento.Actividades = dto.Actividades;
                    entidadDepartamento.InverseIdSuperiorNavigation = dto.InverseIdSuperiorNavigation;
                    entidadDepartamento.IdSuperior = dto.IdSuperior;
                    
                    Repository.Update(entidadDepartamento);
                    return Ok();
                }
            }
            return BadRequest(results.Errors.Select(x => x.ErrorMessage));

        }


        [HttpDelete("{Id}")]
        public IActionResult Delete(int id)
        {
            var entidadDepartamento = Repository.GetById(id);

            if(entidadDepartamento == null)
            {
                return NotFound();
            }
            else
            {
                Repository.Update(entidadDepartamento);
                return Ok();

            }


        }



    }
}
