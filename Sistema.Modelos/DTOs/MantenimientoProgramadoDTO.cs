using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistema.Modelos.Modelos;

namespace Sistema.Modelos.DTOs
{
    public class MantenimientoProgramadoDTO
    {
        public int Codigo { get; set; }
        public DateTime FechaProgramada { get; set; }
        public DateTime FechaRealizada { get; set; }
        public MantenimientoProgramado.TipoMantenimiento Tipo { get; set; }
        public int CamionCodigo { get; set; }
        public int TallerCodigo { get; set; }
        public int Kilometraje { get; set; }
    }

}
