using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema.Modelos.Modelos
{
    public class Licencia
    {
        public enum Tipo { A = 1, B = 2, C = 3, D = 4, E = 5, F = 6, G = 7 }
        public enum EstadoLicencia { Activa = 1, Inactiva = 2, Suspendida = 3, Revocada = 4 }
        [Key] public int Codigo { get; set; }
        public Tipo TipoLicencia { get; set; }
        public EstadoLicencia Estado { get; set; }
        public DateTime FechaEmision { get; set; }
        public DateTime FechaVencimiento { get; set; }
        
        //foreign key
        public int ConductorCodigo { get; set; }
        //navegacion
        public Conductor? Conductor { get; set; }
    }
}
