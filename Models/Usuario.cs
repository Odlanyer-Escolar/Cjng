using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cjng.Models
{
    public class Usuario
    {
        
        public int Id { get; set; } 
        public string Contrasena { get; set; } = null!;
        public string usuario { get; set; } = null!;
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public RolUsuario Rol { get; set; } = RolUsuario.Contador;
    }

    public enum RolUsuario { 
    Admin, Contador
    }
}
