using APIActividadesITESRC.Models.Entities;
using System.Collections.ObjectModel;

namespace APIActividadesITESRC.Models.DTOs
{
    public class DepartamentoDTO
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        public int? IdSuperior { get; set; }
    }
}
