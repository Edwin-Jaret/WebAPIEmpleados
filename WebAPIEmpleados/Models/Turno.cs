namespace WebAPIEmpleados.Models
{
    public class Turno
    {
        public int Id { get; set; }
        public int EmpleadoId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public TimeSpan? TiempoTotal { get; set; }
    }
}
