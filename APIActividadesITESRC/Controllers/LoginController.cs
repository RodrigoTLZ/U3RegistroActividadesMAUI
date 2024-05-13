using APIActividadesITESRC.Helper;
using APIActividadesITESRC.Models.DTOs;
using APIActividadesITESRC.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIActividadesITESRC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public DepartamentosRepository Repository { get; }

        public LoginController(DepartamentosRepository repository)
        {
            Repository = repository;
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
                    GeneradorToken Jwttoken = new();
                    return Ok(Jwttoken.GetToken(dto.Nombre));
                }
                else
                {
                    return Unauthorized();
                }
            }
        }


    }
}
