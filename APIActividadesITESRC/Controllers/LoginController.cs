using APIActividadesITESRC.Helper;
using APIActividadesITESRC.Models.DTOs;
using APIActividadesITESRC.Models.Entities;
using APIActividadesITESRC.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace APIActividadesITESRC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public DepartamentosRepository Repository { get; }
        public GeneradorToken jwtHelper;
        public Departamentos usuario;

        public LoginController(DepartamentosRepository repository, GeneradorToken jwtHelper)
        {
            Repository = repository;
            this.jwtHelper = jwtHelper;
        }

        [HttpPost]
        public IActionResult Login(LoginDTO dto)
        {
            usuario = Repository.GetAll().FirstOrDefault(x => x.Username == dto.Username && x.Password == Encriptacion.EncriptarSHA512(dto.Password));

            if(usuario == null)
            {
                return Unauthorized();
            }
                    var token = jwtHelper.GetToken(usuario.Username,
                        usuario.IdSuperior == null ? "Admin":"Departamento",
                        usuario.IdSuperior,
                        usuario.Id,
                        new List<Claim> { new Claim("Id", usuario.Id.ToString()) });

                    return Ok(token);
        }


        [HttpGet("GetDepartamentoId")]
        public IActionResult GetDepartamentoId()
        {
            if(usuario != null)
            {
                return Ok(usuario.Id);
            }
            else
            {
                return BadRequest("Ha ocurrido un problema al tratar de obtener el ID del departamento.");
            }
        }
    }
}
