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
        public ActividadesRepository ActividadesRepository { get; set; }
        public DepartamentosAPIController(DepartamentosRepository repository, ActividadesRepository actividadesRepository)
        {
            Repository = repository;   
            ActividadesRepository = actividadesRepository;
        }

        [HttpPost("Agregar")]
        [Authorize(Roles = "Admin")]
        public IActionResult AgregarDepartamento(DepartamentoDTO dto)
        {
            try
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
            catch (Exception)
            {
                return BadRequest("Ha ocurrido un error al tratar de agregar el departamento.");
            }
            
        }


        [HttpGet("ObtenerDepartamentos")]
        [Authorize(Roles = "Departamento,Admin")]

        public IActionResult GetAllDepartamentos()
        {
            try
            {
                var departamentos = Repository.GetAll().Where(x => x.Username.Contains("@equipo7")).Select(x => new DepartamentoDTO
                {
                    Id = x.Id,
                    Nombre = x.Nombre,
                    IdSuperior = x.IdSuperior ?? 0,
                    Username = x.Username,
                    Password = x.Password
                });

                return Ok(departamentos);
            }
            catch (Exception)
            {

                return BadRequest("Ha ocurrido un problema al tratar de obtener los departamentos.");
            }
             
        }

        [HttpGet("ObtenerDepartamentosBasico")]
        [Authorize(Roles = "Departamento,Admin")]

        public IActionResult GetAllDepartamentosBasico()
        {
            try
            {
                var departamentos = Repository.GetAll().Where(x => x.Username.Contains("@equipo7")).Select(x => new DepartamentoDTO
                {
                    Id = x.Id,
                    Username = x.Username
                });

                return Ok(departamentos);
            }
            catch (Exception)
            {

                return BadRequest("Ha ocurrido un problema al tratar de obtener los departamentos.");
            }

        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult EditarDepartemento(DepartamentoDTO dto)
        {
            try
            {
                DepartamentoValidator validator = new(Repository.Context);
                var results = validator.Validate(dto);
                if (results.IsValid)
                {
                    var departamento = Repository.GetById(dto.Id);
                    if (departamento != null)
                    {
                        departamento.Nombre = dto.Nombre;
                        departamento.Username = dto.Username;
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
            catch (Exception)
            {
                return BadRequest("Ha ocurrido un error al tratar de editar el departamento.");
            }
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult EliminarDepartamento(int id)
        {
            using var transaction = Repository.Context.Database.BeginTransaction();

            try
            {
                var entidadDepartamento = Repository.GetById(id);

                if (entidadDepartamento == null)
                {
                    return NotFound();
                }

                var actividadesDepartamento = ActividadesRepository.GetAll().Where(x => x.IdDepartamento == id).ToList();

                if (actividadesDepartamento.Any())
                {
                    foreach (var item in actividadesDepartamento)
                    {
                        ActividadesRepository.Delete(item);
                    }

                    ActividadesRepository.Save();
                }

                Repository.Delete(entidadDepartamento);

                transaction.Commit();

                return Ok();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return StatusCode(500, "Ha ocurrido un problema: " + ex.Message);
            }
        }

    }
}
