using System.ComponentModel.DataAnnotations;

namespace Logs.API.Models
{
    public class RegistroLog
    {
        [Key] public int Codigo { get; set; }
        public DateTime FechaHora { get; set; }
        public string Mensaje { get; set; }
        public int TipoLogCodigo { get; set; }
        public TipoLog? TipoLog { get; set; }
        public string PlacaCamion { get; set; }
    }
}
