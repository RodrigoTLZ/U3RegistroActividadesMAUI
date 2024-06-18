using APIActividadesITESRC.Models.DTOs;
using APIActividadesITESRC.Models.Entities;
using APIActividadesITESRC.Models.Validators;
using APIActividadesITESRC.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace APIActividadesITESRC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActividadesAPIController : ControllerBase
    {
        public ActividadesRepository Repository { get; }
        public DepartamentosRepository depasRepository { get; }
        public ActividadesAPIController(ActividadesRepository repository, DepartamentosRepository departamentosRepository)
        {
            Repository = repository;
            this.depasRepository = departamentosRepository;
        }


        [HttpPost("Publicar")]
        [Authorize(Roles = "Departamento,Admin")]
        public IActionResult PublicarActividad(ActividadDTO dto)
        {
            ActividadValidator validator = new();
            var resultados = validator.Validate(dto);
            if(resultados.IsValid)
            {
                var currentTime = DateTime.UtcNow;

                Actividades entity = new()
                {
                    Descripcion = dto.Descripcion,
                    FechaCreacion = currentTime,
                    FechaActualizacion = currentTime,
                    IdDepartamento = dto.IdDepartamento,
                    Estado = 1,
                    Titulo = dto.Titulo,
                    FechaRealizacion = dto.FechaRealizacion.HasValue ? new DateOnly(dto.FechaRealizacion.Value.Year, dto.FechaRealizacion.Value.Month, dto.FechaRealizacion.Value.Day) : DateOnly.FromDateTime(DateTime.Now)
            };
                Repository.Insert(entity);

                if(dto.Imagen != null)
                {
                    SubirImagen(dto.Imagen, entity.Id);
                }
                return Ok();
            }
            else
            {
                return BadRequest(resultados.Errors.Select(x => x.ErrorMessage));
            }
        }

        private void SubirImagen(string imagen, int id)
        {
            try
            {
                var imageBytes = Convert.FromBase64String(imagen);
                var directorio = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Imagenes", User.Identity.Name);
                if (!Directory.Exists(directorio))
                {
                    Directory.CreateDirectory(directorio);
                }
                var archivo = $"{id}.jpg";
                var archivocompleto = Path.Combine(directorio, archivo);
                System.IO.File.WriteAllBytes(archivocompleto, imageBytes);
            }
            catch (Exception)
            {

            }
        }

        private string ObtenerImagen(int id, string nombre)
        {
            try
            {
                var directorio = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Imagenes", nombre, $"{id}.jpg");
                if (System.IO.File.Exists(directorio))
                {
                    var imageBytes = System.IO.File.ReadAllBytes(directorio);
                    return Convert.ToBase64String(imageBytes);
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                return $"Ha ocurrido un problema al obtener la imagen: {ex.Message}";
            }
        }


        [HttpGet("{fecha?}/{hora?}/{minutos?}/{id}")]
        [Authorize(Roles = "Departamento,Admin")]
        public IActionResult GetAllActividades(DateTime? fecha, int hora = 0, int minutos = 0, int id = 0)
        {
            if (fecha != null)
            {
                DateTime date = new DateTime(fecha.Value.Year, fecha.Value.Month, fecha.Value.Day, hora, minutos, 0);
            }
            var departamento = depasRepository.GetById(id);

            if(departamento != null)
            {
                if (User.FindFirstValue("IdDepartamentoSuperior") != "")
                {
                    List<ActividadDTO> ListaActividades = new();

                    if (departamento != null)
                    {
                        GetActividadesSubordinados(departamento, ListaActividades, fecha);
                        return Ok(ListaActividades);
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                var actividades = Repository.GetAll().Where(x => fecha == null || x.FechaActualizacion > fecha || x.FechaCreacion > fecha).Where(x => x.IdDepartamentoNavigation.Username.Contains("@equipo7")).Select(x => new ActividadDTO
                {
                    Id = x.Id,
                    Titulo = x.Titulo,
                    Descripcion = x.Descripcion,
                    FechaRealizacion = x.FechaRealizacion.HasValue ? new DateTime(x.FechaRealizacion.Value.Year, x.FechaRealizacion.Value.Month, x.FechaRealizacion.Value.Day) : DateTime.Now,
                    IdDepartamento = x.IdDepartamento,
                    Estado = x.Estado,
                    Imagen = ObtenerImagen(x.Id, x.IdDepartamentoNavigation.Username),
                    FechaActualizacion = x.FechaActualizacion,
                    FechaCreacion = x.FechaCreacion
                });

                return Ok(actividades);
            }
            return BadRequest("Ha ocurrido un problema al tratar de obtener las actividades.");
            
        }


        [Authorize(Roles = "Departamento,Admin")]
        private void GetActividadesSubordinados(Departamentos departamento, List<ActividadDTO> ListadoActividades, DateTime? fecha)
        {

            var actividadesActuales = Repository.GetAll()
                .Where(x => (fecha == null || x.FechaActualizacion > fecha || x.FechaCreacion > fecha) && x.IdDepartamento == departamento.Id)
                .Select(x => new ActividadDTO
                {
                    Id = x.Id,
                    IdDepartamento = x.IdDepartamento,
                    Titulo = x.Titulo,
                    Descripcion = x.Descripcion ?? "",
                    Estado = x.Estado,
                    Imagen = ObtenerImagen(x.Id, departamento.Username),
                    FechaActualizacion = x.FechaActualizacion,
                    FechaCreacion = x.FechaCreacion,
                    FechaRealizacion = x.FechaRealizacion.HasValue ? x.FechaRealizacion.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null,
                })
                .ToList();

            ListadoActividades.AddRange(actividadesActuales);

            foreach (var item in departamento.InverseIdSuperiorNavigation)
            {
                GetActividadesSubordinados(item, ListadoActividades, fecha);
            }
        }


        [HttpPost("GuardarBorrador")]
        [Authorize(Roles = "Departamento,Admin")]
        public IActionResult GuardarBorrador(ActividadDTO dto)
        {
            try
            {
                BorradorValidator validator = new();
                var results = validator.Validate(dto);
                if (results.IsValid)
                {
                    var currentTime = DateTime.UtcNow;
                    Actividades entity = new()
                    {
                        Descripcion = dto.Descripcion,
                        FechaCreacion = currentTime,
                        FechaActualizacion = currentTime,
                        IdDepartamento = dto.IdDepartamento,
                        Estado = 0,
                        Titulo = dto.Titulo,
                        FechaRealizacion = dto.FechaRealizacion.HasValue ? new DateOnly(dto.FechaRealizacion.Value.Year, dto.FechaRealizacion.Value.Month, dto.FechaRealizacion.Value.Day) : DateOnly.FromDateTime(DateTime.Now)
                    };
                    Repository.Insert(entity);
                    return Ok();
                }
                else
                {
                    return BadRequest(results.Errors.Select(x => x.ErrorMessage));
                }
            }
            catch (Exception)
            {
                return BadRequest("Ha ocurrido un problema al intentar guardar el borrador.");
            } 
        }


        [HttpPost("PublicarBorrador")]
        [Authorize(Roles = "Departamento,Admin")]
        public IActionResult PublicarBorrador(ActividadDTO dto)
        {
            try
            {
                ActividadValidator validator = new();
                var results = validator.Validate(dto);
                if (results.IsValid)
                {
                    var entidadActividad = Repository.GetById(dto.Id);
                    if (entidadActividad == null || entidadActividad.Estado == 2)
                    {
                        return NotFound();
                    }
                    else
                    {
                        entidadActividad.Titulo = dto.Titulo;
                        entidadActividad.Descripcion = dto.Descripcion;
                        entidadActividad.Estado = 1;
                        entidadActividad.FechaCreacion = DateTime.UtcNow;
                        entidadActividad.FechaActualizacion = DateTime.UtcNow;
                        entidadActividad.FechaRealizacion = dto.FechaRealizacion.HasValue ? new DateOnly(dto.FechaRealizacion.Value.Year, dto.FechaRealizacion.Value.Month, dto.FechaRealizacion.Value.Day) : DateOnly.FromDateTime(DateTime.Now);
                        Repository.Update(entidadActividad);


                        if (dto.Imagen != null)
                        {
                            SubirImagen(dto.Imagen, entidadActividad.Id);
                        }
                        return Ok();

                    }
                }
                else
                {
                    return BadRequest(results.Errors.Select(x => x.ErrorMessage));
                }
            }
            catch (Exception)
            {
                return BadRequest("Ha ocurrido un problema al trata de publicar el borrador.");
            }
        }

        [HttpPut("{Id}")]
        [Authorize(Roles = "Departamento,Admin")]
        public IActionResult EditarActividad(ActividadDTO dto)
        {
            ActividadValidator validator = new();
            var results = validator.Validate(dto);
            if (results.IsValid)
            {
                var entidadActividad = Repository.GetById(dto.Id);
                if(entidadActividad == null || entidadActividad.Estado == 2)
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

                    if (dto.Imagen != null)
                    {
                        SubirImagen(dto.Imagen, entidadActividad.Id);
                    }
                    return Ok();
                }
            }
            else
            {
                return BadRequest(results.Errors.Select(x => x.ErrorMessage));
            }
        }


        [HttpDelete("{Id}")]
        [Authorize(Roles = "Departamento,Admin")]
        public IActionResult EliminarActividad(int id)
        {

            try
            {
                var entidadActividad = Repository.GetById(id);
                if (entidadActividad == null)
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
            catch (Exception)
            {

                return BadRequest("Ha ocurrido un problema al tratar de eliminar la actividad.");
            }
        }
    }
}
