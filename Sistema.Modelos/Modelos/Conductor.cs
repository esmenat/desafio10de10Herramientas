using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema.Modelos.Modelos
{
    public enum Genero { 
        Femenino = 1, 
        Masculino = 2}

    public enum TipoSangre
    {
        OPositivo = 1,
        ONegativo = 2,
        APositivo = 3,
        ANegativo = 4,
        BPositivo = 5,
        BNegativo = 6,
        ABPositivo = 7,
        ABNegativo = 8
    }

    public class Conductor
    {
        [Key]public int Codigo { get; set; }
        public string Nombre { get; set; }

        public string Nacionalidad { get; set; }
        public int Edad { get; set; }
        public  Genero Genero { get; set; }
        public TipoSangre TipoSangre { get; set; }

        //foreign key
        public int CamionCodigo { get; set; }
        //navegacion
        public Camion? Camion { get; set; }
        //nav
        public List<Licencia>? Licencias { get; set; }


    }
}
