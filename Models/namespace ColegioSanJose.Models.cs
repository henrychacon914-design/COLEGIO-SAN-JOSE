using Microsoft.EntityFrameworkCore;

namespace ColegioSanJose.Models
{
    public class ColegioDbContext : DbContext
    {
        public ColegioDbContext
            (DbContextOptions<ColegioDbContext> options)
            : base(options)
        {
        }

        public DbSet<Alumno> Alumno { get; set; }

        public DbSet<Materia> Materia { get; set; }

        public DbSet<Expediente> Expediente { get; set; }
    }
}