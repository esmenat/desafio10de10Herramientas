using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistema.Modelos.Modelos;

namespace Sistema.Modelos.DTOs
{
    public class LicenciaDTO
    {
        public int Codigo { get; set; }
        public Licencia.Tipo TipoLicencia { get; set; }
        public Licencia.EstadoLicencia Estado { get; set; }
        public DateTime FechaEmision { get; set; }
        public DateTime FechaVencimiento { get; set; }
    }

}
