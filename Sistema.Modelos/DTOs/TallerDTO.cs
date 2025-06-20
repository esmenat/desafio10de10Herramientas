using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistema.Modelos.Modelos;

namespace Sistema.Modelos.DTOs
{
    public class TallerDTO
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public string Ciudad { get; set; }
        public int CapacidadMaximaDeReparacionesSimultaneas { get; set; }

    }

}
