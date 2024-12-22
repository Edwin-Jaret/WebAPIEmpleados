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
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=EDWIN-LAP;Database=TurnosDB;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Relación entre turno y empleado
            modelBuilder.Entity<Turno>()
                .HasOne<Empleados>()
                .WithMany()
                .HasForeignKey(t => t.EmpleadoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
