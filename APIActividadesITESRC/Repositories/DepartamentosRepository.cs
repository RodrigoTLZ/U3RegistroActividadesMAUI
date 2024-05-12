using APIActividadesITESRC.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace APIActividadesITESRC.Repositories
{
    public class DepartamentosRepository
    {
        public ItesrcneActividadesContext Context { get; set; }
        public DepartamentosRepository(ItesrcneActividadesContext context)
        {
            Context = context;
        }

        public IEnumerable<Departamentos> GetAll()
        {
            return Context.Departamentos.OrderBy(x => x.Nombre).Include(x=>x.InverseIdSuperiorNavigation);
        }

        public Departamentos? GetById(int id)
        {
            return Context.Departamentos.Find(id);
        }

        public void Insert(Departamentos departamento)
        {
            Context.Departamentos.Add(departamento);
            Context.SaveChanges();
        }

        public void Update(Departamentos departamento)
        {
            Context.Departamentos.Update(departamento);
            Context.SaveChanges();
        }

        public void Delete(Departamentos departamento)
        {
            Context.Remove(departamento);
            Context.SaveChanges();
        }


        public bool CompararNombre(string text)
        {
            return !Context.Departamentos.Any(x => x.Nombre == text);
        }

        public bool CompararUser(string text)
        {
            return !Context.Departamentos.Any(x => x.Username == text);
        }

    }
}
