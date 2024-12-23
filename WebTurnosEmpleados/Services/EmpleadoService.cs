using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WebTurnosEmpleados.Models;

namespace WebTurnosEmpleados.Services
{
    public class EmpleadoService
    {
        private readonly HttpClient _httpClient;
        private Empleados? _empleadoActual;

        public EmpleadoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> CheckDatabaseConnection()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://localhost:44304/api/Empleados/ConexiónBD");
                var content = await response.Content.ReadAsStringAsync();

                System.Diagnostics.Debug.WriteLine($"Respuesta: {content}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                // Log del error específico
                System.Diagnostics.Debug.WriteLine($"Error de conexión: {ex.Message}");
                return false;
            }
        }

        public async Task<Empleados?> Login(string user, string password)
        {
            try
            {
                var empleado = new Empleados
                {
                    Useer = user,
                    Password = password
                };

                var response = await _httpClient.PostAsJsonAsync("https://localhost:44304/api/Empleados/login", empleado);

                if (response.IsSuccessStatusCode)
                {
                    var empleadoLogueado = await response.Content.ReadFromJsonAsync<Empleados>();
                    if (empleadoLogueado != null)
                    {
                        _empleadoActual = empleadoLogueado;
                        return empleadoLogueado;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en login: {ex.Message}");
                return null;
            }
        }
        public async Task<List<Turno>> GetHistorialTurnos(int empleadoId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://localhost:44304/api/Empleados/historial/{empleadoId}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<Turno>>() ?? new List<Turno>();
                }
                return new List<Turno>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al obtener historial: {ex.Message}");
                return new List<Turno>();
            }
        }

        public async Task<Turno?> GetTurnoActivo(int empleadoId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"https://localhost:44304/api/Empleados/turno-activo/{empleadoId}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Turno>();
                }
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al obtener turno activo: {ex.Message}");
                return null;
            }
        }

        public async Task<Turno?> IniciarTurno(int empleadoId)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"https://localhost:44304/api/Empleados/iniciar-turno",
                    new { EmpleadoId = empleadoId });

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Turno>();
                }
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al iniciar turno: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> TerminarTurno(int empleadoId)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"https://localhost:44304/api/Empleados/terminar-turno",
                    new { EmpleadoId = empleadoId });

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error al terminar turno: {ex.Message}");
                return false;
            }
        }

        public Empleados? GetEmpleadoActual()
        {
            return _empleadoActual;
        }
    }
}
