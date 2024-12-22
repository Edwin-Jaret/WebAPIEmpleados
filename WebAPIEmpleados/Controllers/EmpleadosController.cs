using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIEmpleados.Data;
using WebAPIEmpleados.Models;

namespace WebAPIEmpleados.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmpleadosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Estado de conexión con el servidor
        [HttpGet]
        [Route("ConexiónServidor")]
        public ActionResult GetConexiónServidor()
        {
            return Ok("Conexión exitosa con el servidor");
        }

        // Estado de conexión con la base de datos
        [HttpGet]
        [Route("ConexiónBD")]
        public async Task<ActionResult> GetConexiónBD()
        {
            try
            {
                var empleados = await _context.Empleados.ToListAsync();
                return Ok("Conexión exitosa con la base de datos");
            }
            catch (Exception ex)
            {
                return BadRequest("Error al conectar con la base de datos: " + ex.Message);
            }
        }

        // Login de empleados
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login(Empleados empleado)
        {
            try
            {
                var empleadoLogin = await _context.Empleados
                    .FirstOrDefaultAsync(x => x.Useer == empleado.Useer &&
                                            x.Password == empleado.Password);

                if (empleadoLogin == null)
                {
                    return BadRequest("Usuario o contraseña incorrectos");
                }

                // No devolver la contraseña
                empleadoLogin.Password = "";

                return Ok(empleadoLogin);
            }
            catch (Exception ex)
            {
                return BadRequest("Error al intentar iniciar sesión: " + ex.Message);
            }
        }

        // Obtener empleado por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Empleados>> GetEmpleado(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);

            if (empleado == null)
            {
                return NotFound("Empleado no encontrado");
            }

            // No devolver la contraseña
            empleado.Password = "";

            return empleado;
        }

        [HttpGet("historial/{empleadoId}")]
        public async Task<ActionResult<List<Turno>>> GetHistorial(int empleadoId)
        {
            try
            {
                var turnos = await _context.Turnos
                    .Where(t => t.EmpleadoId == empleadoId)
                    .OrderByDescending(t => t.FechaInicio)
                    .ToListAsync();
                return Ok(turnos);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener historial: {ex.Message}");
            }
        }

        [HttpGet("turno-activo/{empleadoId}")]
        public async Task<ActionResult<Turno>> GetTurnoActivo(int empleadoId)
        {
            try
            {
                var turno = await _context.Turnos
                    .Where(t => t.EmpleadoId == empleadoId && t.FechaFin == null)
                    .FirstOrDefaultAsync();
                return Ok(turno);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener turno activo: {ex.Message}");
            }
        }

        [HttpPost("iniciar-turno")]
        public async Task<ActionResult<Turno>> IniciarTurno([FromBody] TurnoRequest request)
        {
            try
            {
                var nuevoTurno = new Turno
                {
                    EmpleadoId = request.EmpleadoId,
                    FechaInicio = DateTime.Now
                };
                _context.Turnos.Add(nuevoTurno);
                await _context.SaveChangesAsync();
                return Ok(nuevoTurno);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al iniciar turno: {ex.Message}");
            }
        }

        [HttpPost("terminar-turno")]
        public async Task<ActionResult> TerminarTurno([FromBody] TurnoRequest request)
        {
            try
            {
                var turnoActivo = await _context.Turnos
                    .Where(t => t.EmpleadoId == request.EmpleadoId && t.FechaFin == null)
                    .FirstOrDefaultAsync();

                if (turnoActivo == null)
                    return BadRequest("No hay turno activo");

                turnoActivo.FechaFin = DateTime.Now;
                turnoActivo.TiempoTotal = turnoActivo.FechaFin.Value - turnoActivo.FechaInicio;
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al terminar turno: {ex.Message}");
            }
        }
    }

    public class TurnoRequest
    {
        public int EmpleadoId { get; set; }
    }
}
