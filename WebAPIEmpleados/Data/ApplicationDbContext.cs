using Microsoft.EntityFrameworkCore;
using WebAPIEmpleados.Models;

namespace WebAPIEmpleados.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Empleados> Empleados { get; set; }
        public DbSet<Turno> Turnos { get; set; }  
    }
}
