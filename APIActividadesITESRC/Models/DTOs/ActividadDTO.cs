using APIActividadesITESRC.Models.Entities;

namespace APIActividadesITESRC.Models.DTOs
{
    public class ActividadDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string? Descripcion { get; set; }
        public DateTime? FechaRealizacion { get; set; }
        public int IdDepartamento { get; set; }
        public string? Imagen { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public int Estado { get; set; }

    }
}
