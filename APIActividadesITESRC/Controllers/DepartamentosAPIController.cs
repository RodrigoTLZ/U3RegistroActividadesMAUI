using APIActividadesITESRC.Models.DTOs;
using APIActividadesITESRC.Models.Entities;
using APIActividadesITESRC.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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


        [HttpPost]

        public IActionResult Post(DepartamentoDTO dto)
        {
            Departamentos entity = new()
            {
                Id = 0,
                Actividades = dto.Actividades,
                IdSuperior = dto.IdSuperior,
                IdSuperiorNavigation = dto.IdSuperiorNavigation,
                InverseIdSuperiorNavigation = dto.InverseIdSuperiorNavigation,
                Nombre = dto.Nombre,
                Password    = dto.Password,
                Username = dto.Username
            };
            Repository.Insert(entity);
            return Ok();
        }

    }
}
