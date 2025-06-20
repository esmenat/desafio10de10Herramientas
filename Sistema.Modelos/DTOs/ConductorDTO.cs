using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistema.Modelos.Modelos;

namespace Sistema.Modelos.DTOs
{
    public class ConductorDTO
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public string Nacionalidad { get; set; }
        public int Edad { get; set; }
        public Genero Genero { get; set; }
        public TipoSangre TipoSangre { get; set; }
    }

}
