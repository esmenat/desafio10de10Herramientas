using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema.Modelos.Modelos
{
    public class Taller
    {

        [Key] public int Codigo { get; set; }
        public string Nombre { get; set; }
        public string Ciudad { get; set; }
        public int CapacidadMaximaDeReparacionesSimultaneas { get; set; }
    
        //navegacion
        public List<MantenimientoProgramado>? MantenimientosProgramados { get; set; }
        

    }
}
