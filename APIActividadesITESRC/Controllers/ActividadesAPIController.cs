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
        public IActionResult Post(ActividadDTO dto)
        {
            ActividadValidator validator = new();
            var resultados = validator.Validate(dto);

            if(resultados.IsValid)
            {
                Actividades entity = new()
                {
                    Id = 0,
                    Descripcion = dto.Descripcion,
                    FechaCreacion = DateTime.UtcNow,
                    FechaActualizacion = DateTime.UtcNow,
                    IdDepartamento = dto.IdDepartamento,
                    //  IdDepartamentoNavigation = dto.IdDepartamentoNavigation
                    Estado = dto.Estado,
                    Titulo = dto.Titulo,
                    FechaRealizacion = dto.FechaRealizacion,
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
            var actividades = Repository.GetAll().Select(x=> new ActividadDTO
            {
                Titulo =x.Titulo,
                Descripcion=x.Descripcion,
                Estado=x.Estado,
                FechaActualizacion=x.FechaActualizacion,
                FechaCreacion =x.FechaCreacion,
                FechaRealizacion=x.FechaRealizacion,
                Id = x.Id,
                IdDepartamento=x.IdDepartamento,
               // IdDepartamentoNavigation = x.IdDepartamentoNavigation
            });

            return Ok(actividades);
        }


        [HttpPut("{Id}")]
        public IActionResult Put(ActividadDTO dto)
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
            return BadRequest(results.Errors.Select(x => x.ErrorMessage));

        }


        [HttpDelete("{Id}")]
        public IActionResult Delete(int id)
        {
            var entidadActividad = Repository.GetById(id);

            if(entidadActividad == null)
            {
                return NotFound();
            }

            //entidadActividad.Estado = "Eliminado"
            entidadActividad.FechaActualizacion = DateTime.UtcNow;
            Repository.Update(entidadActividad);
            return Ok();
        }


    }
}
