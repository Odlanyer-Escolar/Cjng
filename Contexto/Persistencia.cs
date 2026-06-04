using Cjng.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cjng.Contexto
{
    public static class Persistencia
    {
        public static Usuario? UsuarioActual { get; set; } = null;
    }
}
