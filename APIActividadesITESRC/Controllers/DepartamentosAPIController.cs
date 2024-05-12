using APIActividadesITESRC.Models.DTOs;
using APIActividadesITESRC.Models.Entities;
using APIActividadesITESRC.Models.Validators;
using APIActividadesITESRC.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Reflection.Metadata.Ecma335;
using System.Text;

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

        [HttpPost("Agregar")]
        public IActionResult AgregarDepartamento(DepartamentoDTO dto)
        {
            DepartamentoValidator validator = new(Repository.Context);
            var resultados = validator.Validate(dto);

            if (resultados.IsValid)
            {
                Departamentos entity = new()
                {
                    Nombre = dto.Nombre,
                    Password = EncriptarSHA512(dto.Password),
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
        public IActionResult GetAllDepartamentos()
        {
            var departamentos = Repository.GetAll().Select(x=> new DepartamentoDTO
            {
                Nombre = x.Nombre,
                IdSuperior= x.IdSuperior??0,
                Username = x.Username,
                Password = x.Password
            });

            return Ok(departamentos);    
        }


        [HttpPut("Editar")]
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
                   departamento.Password = EncriptarSHA512(dto.Password);
                   departamento.IdSuperior = dto.IdSuperior;

                    Repository.Update(departamento);
                    return Ok("Los cambios se han realizado correctamente");
                }
                else
                {
                    return NotFound();
                }
            }
            return BadRequest(results.Errors.Select(x => x.ErrorMessage));

        }


        [HttpDelete("Eliminar")]
        public IActionResult EliminarDepartamento(int id)
        {
            var entidadDepartamento = Repository.GetById(id);

            if(entidadDepartamento == null)
            {
                return NotFound();
            }
            else
            {
                Repository.Delete(entidadDepartamento);
                return Ok();
            }
        }

        static string EncriptarSHA512(string password)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha512.ComputeHash(bytes);

                StringBuilder builder = new StringBuilder();
                foreach (byte item in hash)
                {
                    builder.Append(item.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
