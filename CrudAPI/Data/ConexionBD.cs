using CrudAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CrudAPI.Data
{
    public class ConexionBD : DbContext
    {
        public ConexionBD(DbContextOptions<ConexionBD> options) :base(options) {
            
        } 
        public DbSet<Persona> Personas => Set<Persona>();
    }
}
