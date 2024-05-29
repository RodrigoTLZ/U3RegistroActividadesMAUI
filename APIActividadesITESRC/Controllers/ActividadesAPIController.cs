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
                    FechaRealizacion = dto.FechaRealizacion.HasValue ? new DateOnly(dto.FechaRealizacion.Value.Year, dto.FechaRealizacion.Value.Month, dto.FechaRealizacion.Value.Day) : DateOnly.FromDateTime(DateTime.Now)
            };
                Repository.Insert(entity);
                return Ok();
            }
            else
            {
                return BadRequest(resultados.Errors.Select(x => x.ErrorMessage));
            }
        }

        [HttpGet("{fecha?}/{hora?}/{minutos?}/{id}")]
        public IActionResult GetAllActividades(DateTime? fecha, int hora = 0, int minutos = 0, int id = 0)
        {
            if (fecha != null)
            {
                DateTime date = new DateTime(fecha.Value.Year, fecha.Value.Month, fecha.Value.Day, hora, minutos, 0);
            }

            var actividades = Repository.GetAll().Where(x=> fecha == null || x.FechaActualizacion > fecha).Select(x => new ActividadDTO
            {
                Id = x.Id,
                Titulo = x.Titulo,
                Descripcion = x.Descripcion,
                FechaRealizacion = x.FechaRealizacion.HasValue ? new DateTime(x.FechaRealizacion.Value.Year, x.FechaRealizacion.Value.Month, x.FechaRealizacion.Value.Day) : DateTime.Now,
                IdDepartamento = x.IdDepartamento,
                Estado = x.Estado,
                FechaActualizacion = x.FechaActualizacion,
                FechaCreacion = x.FechaCreacion
            }).Where(x => x.IdDepartamento == id);

            return Ok(actividades);
        }


        [HttpPut("{Id}")]
        public IActionResult EditarActividad(ActividadDTO dto)
        {
           // var actividadesDepartamento = int.Parse(User.Identities.SelectMany(ci => ci.Claims).FirstOrDefault(c => c.Type == "DepartamentoId").Value);

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
                        entidadActividad.Titulo = dto.Titulo;
                        entidadActividad.Descripcion = dto.Descripcion;
                        entidadActividad.Estado = dto.Estado;
                        entidadActividad.FechaActualizacion = DateTime.UtcNow;
                    entidadActividad.FechaRealizacion = dto.FechaRealizacion.HasValue ? new DateOnly(dto.FechaRealizacion.Value.Year, dto.FechaRealizacion.Value.Month, dto.FechaRealizacion.Value.Day) : DateOnly.FromDateTime(DateTime.Now);
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
            else
            {
                entidadActividad.Estado = 2;
                entidadActividad.FechaActualizacion = DateTime.UtcNow;
                Repository.Update(entidadActividad);
                return Ok();
            }
            
        }

    }
}
