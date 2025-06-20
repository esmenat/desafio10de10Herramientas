using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema.Modelos.Modelos
{
    public class Usuario
    {
        [Key] public int Codigo { get; set; }
        public string NombreUsuario { get; set; }
        public string Contraseña {  get; set; }
    }
}
