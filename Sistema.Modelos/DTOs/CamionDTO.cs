using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistema.Modelos.DTOs
{
    public class CamionDTO
    {
        public int Codigo { get; set; }
        public string Modelo { get; set; }
        public int Anio { get; set; }
        public string Placa { get; set; }
        public double KilometrajeActual { get; set; }
        public Estado Estado { get; set; }
    }

}
