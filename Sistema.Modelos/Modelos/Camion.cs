using System.ComponentModel.DataAnnotations;
using Sistema.Modelos.Modelos;

namespace Sistema.Modelos
{
    public enum Estado { 
        Activo = 1,
        EnMantenimiento = 2, 
        Dañado = 3,
        Inactivo = 4 }
    public class Camion
    {
        [Key] public int Codigo { get; set; }
        public string Modelo { get; set; }
        public int Anio { get; set; }
        public string Placa { get; set; }
        public double KilometrajeActual { get; set; }
        public  Estado Estado  { get; set; }

        //navegacion
        public List<Conductor>? Conductores { get; set; }
        public List<MantenimientoProgramado>? MantenimientosProgramados { get; set; }

    }
}
