using APIActividadesITESRC.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace APIActividadesITESRC.Repositories
{
    public class ActividadesRepository
    {
        public ItesrcneActividadesContext Context { get; set; }
        public ActividadesRepository(ItesrcneActividadesContext context)
        {
            Context = context;
        }

        public IEnumerable<Actividades> GetAll()
        {
            return Context.Actividades.OrderBy(x => x.Titulo).Include(x=>x.IdDepartamentoNavigation);
        }

        public Actividades? GetById(int id)
        {
            return Context.Actividades.Find(id);
        }

        public void Insert(Actividades actividad)
        {
            Context.Actividades.Add(actividad);
            Context.SaveChanges();
        }

        public void Update(Actividades actividad)
        {
            Context.Actividades.Update(actividad);
            Context.SaveChanges();
        }

        public void Delete(Actividades actividad)
        {
            Context.Remove(actividad);
            Context.SaveChanges();
        }
    }
}
