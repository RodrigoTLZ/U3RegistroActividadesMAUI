using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;
using System.Threading.Tasks;

namespace APIActividadesMAUI.Models.Entities
{
    [Table("Departamentos")]
    public class Departamento
    {
        [SQLite.PrimaryKey]
        public int Id { get; set; }

        [NotNull]
        public string Nombre { get; set; } = null!;

        [NotNull]
        public string Username { get; set; } = null!;

        [NotNull]
        public string Password { get; set; } = null!;

        public int IdSuperior {  get; set; }
    }
}
