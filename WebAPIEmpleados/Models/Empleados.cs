namespace WebAPIEmpleados.Models
{
    public class Empleados
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public string Useer { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
