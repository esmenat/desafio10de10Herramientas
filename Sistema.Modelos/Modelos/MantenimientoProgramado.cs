using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema.Modelos.Modelos
{

    public class MantenimientoProgramado
    {
        public enum TipoMantenimiento { Preventivo = 1, Correctivo = 2 }
        public enum EstadoMantenimiento { Pendiente = 1, EnProceso = 2, Finalizado = 3, Cancelado = 4 }
        [Key] public int Codigo { get; set; }
        public DateTime FechaProgramada { get; set; }
        public DateTime FechaRealizada { get; set; }
        public TipoMantenimiento Tipo { get; set; }
        public EstadoMantenimiento Estado { get; set; }
        public string Descripcion { get; set; }
        public double Kilometraje { get; set; }
        //foreign key
        public int CamionCodigo { get; set; }
        public int TallerCodigo { get; set; }

        //navegacion
        public Camion? Camion { get; set; }
        public Taller? Taller { get; set; }

    }
}
