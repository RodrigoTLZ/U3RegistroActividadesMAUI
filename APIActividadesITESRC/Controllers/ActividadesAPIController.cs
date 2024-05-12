using APIActividadesITESRC.Models.DTOs;
using APIActividadesITESRC.Models.Entities;
using APIActividadesITESRC.Models.Validators;
using APIActividadesITESRC.Repositories;
using Microsoft.AspNetCore.Http;
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


        [HttpPost]
        public IActionResult PublicarActividad(ActividadDTO dto)
        {
            ActividadValidator validator = new();
            var resultados = validator.Validate(dto);

            if(resultados.IsValid)
            {
                Actividades entity = new()
                {
                    Descripcion = dto.Descripcion,
                    FechaCreacion = DateTime.UtcNow,
                    FechaActualizacion = DateTime.UtcNow,
                    IdDepartamento = dto.IdDepartamento,
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

        [HttpGet]
        public IActionResult GetAllActividades()
        {
            var actividades = Repository.GetAll().Select(x=> new ActividadDTO
            {
                Titulo =x.Titulo,
                Descripcion=x.Descripcion,
                FechaRealizacion=x.FechaRealizacion,
                IdDepartamento=x.IdDepartamento,
            });

            return Ok(actividades);
        }


        [HttpPut("{Id}")]
        public IActionResult EditarActividad(ActividadDTO dto)
        {
            ActividadValidator validator = new();
            var results = validator.Validate(dto);
            if (results.IsValid)
            {
                var entidadActividad = Repository.GetById(dto.Id);
                if(entidadActividad == null)
                {
                    return NotFound();
                }
                else
                {
                    entidadActividad.Titulo = dto.Titulo;
                    entidadActividad.Descripcion = dto.Descripcion;
                    entidadActividad.Estado = dto.Estado;
                    entidadActividad.FechaActualizacion = DateTime.UtcNow;
                    entidadActividad.FechaCreacion=dto.FechaCreacion;

                    Repository.Update(entidadActividad);
                    return Ok();
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
            var entidadActividad = Repository.GetById(id);
            if(entidadActividad == null)
            {
                return NotFound();
            }
            entidadActividad.Estado = 2;
            entidadActividad.FechaActualizacion = DateTime.UtcNow;
            Repository.Update(entidadActividad);
            return Ok();
        }
    }
}
