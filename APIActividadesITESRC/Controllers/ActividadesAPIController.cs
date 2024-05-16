using APIActividadesITESRC.Models.DTOs;
using APIActividadesITESRC.Models.Entities;
using APIActividadesITESRC.Models.Validators;
using APIActividadesITESRC.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace APIActividadesITESRC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActividadesAPIController : ControllerBase
    {
        public ActividadesRepository Repository { get; }
        public ActividadesAPIController(ActividadesRepository repository)
        {
            Repository = repository;
        }


        [HttpPost("Publicar")]
        public IActionResult PublicarActividad(ActividadDTO dto)
        {
            var actividadesDepartamento = int.Parse(User.Identities.SelectMany(ci => ci.Claims).FirstOrDefault(c => c.Type == "DepartamentoId").Value);

            ActividadValidator validator = new();
            var resultados = validator.Validate(dto);

            if(resultados.IsValid)
            {
                Actividades entity = new()
                {
                    Descripcion = dto.Descripcion,
                    FechaCreacion = DateTime.UtcNow,
                    FechaActualizacion = DateTime.UtcNow,
                    IdDepartamento = actividadesDepartamento,
                    Estado = 1,
                    Titulo = dto.Titulo,
                    FechaRealizacion = dto.FechaRealizacion
                };
                Repository.Insert(entity);
                return Ok();
            }
            else
            {
                return BadRequest(resultados.Errors.Select(x => x.ErrorMessage));
            }
        }

        [HttpGet("Obtener")]
        public IActionResult GetAllActividades()
        {
            var actividadesDepartamento = int.Parse(User.Identities.SelectMany(ci => ci.Claims).FirstOrDefault(c => c.Type == "DepartamentoId").Value);

            var actividades = Repository.GetAll().Select(x=> new ActividadDTO
            {
                Id = x.Id,
                Titulo =x.Titulo,
                Descripcion=x.Descripcion,
                FechaRealizacion=x.FechaRealizacion,
                IdDepartamento=x.IdDepartamento,
                Estado=x.Estado,
                FechaActualizacion=x.FechaActualizacion,
                FechaCreacion=x.FechaCreacion
            }).Where(x=>x.IdDepartamento >= actividadesDepartamento);

            return Ok(actividades);
        }

        [HttpGet("ObtenerPorDepartamento")]
        public IActionResult GetAllActividadesPorDepartamento()
        {
            var actividadesDepartamento = int.Parse(User.Identities.SelectMany(ci => ci.Claims).FirstOrDefault(c => c.Type == "DepartamentoId").Value);

            var actividades = Repository.GetAll().Select(x => new ActividadDTO
            {
                Id = x.Id,
                Titulo = x.Titulo,
                Descripcion = x.Descripcion,
                FechaRealizacion = x.FechaRealizacion,
                IdDepartamento = x.IdDepartamento,
                Estado = x.Estado,
                FechaActualizacion = x.FechaActualizacion,
                FechaCreacion = x.FechaCreacion
            }).Where(x => x.IdDepartamento == actividadesDepartamento);

            return Ok(actividades);
        }


        [HttpPut("{Id}")]
        public IActionResult EditarActividad(ActividadDTO dto)
        {
            var actividadesDepartamento = int.Parse(User.Identities.SelectMany(ci => ci.Claims).FirstOrDefault(c => c.Type == "DepartamentoId").Value);

            ActividadValidator validator = new();
            var results = validator.Validate(dto);
            if (results.IsValid)
            {
                var entidadActividad = Repository.GetById(dto.Id);
                if(entidadActividad == null && entidadActividad.Estado == 2)
                {
                    return NotFound();
                }
                else
                {
                    if(entidadActividad.IdDepartamento == actividadesDepartamento)
                    {
                        entidadActividad.Titulo = dto.Titulo;
                        entidadActividad.Descripcion = dto.Descripcion;
                        entidadActividad.Estado = dto.Estado;
                        entidadActividad.FechaActualizacion = DateTime.UtcNow;
                        entidadActividad.FechaRealizacion = dto.FechaRealizacion;
                        Repository.Update(entidadActividad);
                        return Ok();
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }
            }
            else
            {
                return BadRequest(results.Errors.Select(x => x.ErrorMessage));
            }

        }


        [HttpDelete("{Id}")]
        public IActionResult EliminarActividad(int id)
        {
            var actividadesDepartamento = int.Parse(User.Identities.SelectMany(ci => ci.Claims).FirstOrDefault(c => c.Type == "DepartamentoId").Value);

            var entidadActividad = Repository.GetById(id);
            if(entidadActividad == null)
            {
                return NotFound();
            }
            if(id == actividadesDepartamento)
            {
                entidadActividad.Estado = 2;
                entidadActividad.FechaActualizacion = DateTime.UtcNow;
                Repository.Update(entidadActividad);
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
            
        }
    }
}
