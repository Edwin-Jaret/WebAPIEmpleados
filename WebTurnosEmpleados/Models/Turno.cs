﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTurnosEmpleados.Models
{
    public class Turno
    {
       /* public int Id { get; set; }
        public int EmpleadoId { get; set; } */
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public TimeSpan? TiempoTotal { get; set; }
    }
}