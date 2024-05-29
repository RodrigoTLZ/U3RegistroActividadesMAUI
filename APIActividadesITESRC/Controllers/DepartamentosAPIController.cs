using APIActividadesITESRC.Models.DTOs;
using APIActividadesITESRC.Models.Entities;
using APIActividadesITESRC.Models.Validators;
using APIActividadesITESRC.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using APIActividadesITESRC.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;

namespace APIActividadesITESRC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartamentosAPIController : ControllerBase
    {
        public DepartamentosRepository Repository { get; }
        public ActividadesRepository ActividadesRepository { get; }
        public DepartamentosAPIController(DepartamentosRepository repository, ActividadesRepository actividadesRepository)
        {
            Repository = repository;   
            ActividadesRepository = actividadesRepository;
        }

        [HttpPost("Agregar")]
        [Authorize(Roles = "Admin")]
        public IActionResult AgregarDepartamento(DepartamentoDTO dto)
        {
            DepartamentoValidator validator = new(Repository.Context);
            var resultados = validator.Validate(dto);

            if (resultados.IsValid)
            {
                Departamentos entity = new()
                {
                    Nombre = dto.Nombre,
                    Password = Encriptacion.EncriptarSHA512(dto.Password),
                    Username = dto.Username,
                    IdSuperior = dto.IdSuperior,
                };
                Repository.Insert(entity);
                return Ok();
            }
            else
            {
                return BadRequest(resultados.Errors.Select(x => x.ErrorMessage));
            }
        }


        [HttpGet("ObtenerDepartamentos")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllDepartamentos()
        {
            var departamentos = Repository.GetAll().Select(x=> new DepartamentoDTO
            {
                Id = x.Id,
                Nombre = x.Nombre,
                IdSuperior= x.IdSuperior??0,
                Username = x.Username,
                Password = x.Password
            });

            return Ok(departamentos);    
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult EditarDepartemento(DepartamentoDTO dto)
        {
            DepartamentoValidator validator = new(Repository.Context);
            var results = validator.Validate(dto);
            if (results.IsValid)
            {
                var departamento = Repository.GetById(dto.Id);
                if(departamento != null)
                {
                   departamento.Nombre = dto.Nombre;
                   departamento.Password = Encriptacion.EncriptarSHA512(dto.Password);
                   departamento.IdSuperior = dto.IdSuperior;

                    Repository.Update(departamento);
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            return BadRequest(results.Errors.Select(x => x.ErrorMessage));

        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult EliminarDepartamento(int id)
        {
            var entidadDepartamento = Repository.GetById(id);

            if(entidadDepartamento == null)
            {
                return NotFound();
            }
            else
            {
                var actividadesdepartamento = ActividadesRepository.GetAll().Where(x=>x.IdDepartamento == id);
                if (actividadesdepartamento.Any())
                {
                    foreach (var item in actividadesdepartamento)
                    {
                        ActividadesRepository.Delete(item);
                    }
                    Repository.Delete(entidadDepartamento);
                    return Ok();
                }
            }
            return NotFound();
        }

    }
}
