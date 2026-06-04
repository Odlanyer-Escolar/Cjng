using System;
using System.Collections.Generic;
using System.Text;

namespace Cjng.Models
{
    public class Trabajador
    {
        public int Id { get; set; }
        public string Cargo { get; set; } = null!;
        public double Salario { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime Horas { get; set; }
        public bool Estado { get; set; } = true;
    }
}
