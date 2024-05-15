using APIActividadesITESRC.Helper;
using APIActividadesITESRC.Models.DTOs;
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

        public LoginController(DepartamentosRepository repository, GeneradorToken jwtHelper)
        {
            Repository = repository;
            this.jwtHelper = jwtHelper;
        }

        [HttpPost]
        public IActionResult Login(DepartamentoDTO dto)
        {
            var usuarioexistente = Repository.GetByEmail(dto.Username);

            if(usuarioexistente == null)
            {
                return NotFound();
            }
            else
            {
                if(usuarioexistente.Password == Encriptacion.EncriptarSHA512(dto.Password))
                {
                    var token = jwtHelper.GetToken(usuarioexistente.Username,
                        usuarioexistente.Nombre,
                        new List<Claim> { new Claim("Id", usuarioexistente.Id.ToString()) });
                    return Ok();
                }
                else
                {
                    return Unauthorized();
                }
            }
        }


    }
}
