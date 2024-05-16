using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIActividadesMAUI.Models.Entities
{
    public class Actividad
    {
        [SQLite.PrimaryKey]
        public int Id { get; set; }

        [NotNull]
        public string Titulo { get; set; } = null!;

        public string Descripcion { get; set; } = null!;

        public DateTime FechaRealizacion { get; set; }

        [NotNull]
        public int IdDepartamento { get; set; }

        [NotNull]
        public DateTime FechaCreacion { get; set; }

        [NotNull]
        public DateTime FechaActualizacion { get; set; }

        [NotNull]
        public int Estado { get; set; }

    }
}
