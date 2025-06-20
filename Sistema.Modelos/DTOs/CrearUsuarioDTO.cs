using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema.Modelos.DTOs
{
    public class CrearUsuarioDTO
    {
        [Required]
        public string NombreUsuario { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string Contraseña { get; set; }
    }

}
