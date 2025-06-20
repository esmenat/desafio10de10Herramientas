using System.ComponentModel.DataAnnotations;

namespace Logs.API.Models
{
    public class TipoLog
    {
        [Key] public int Codigo { get; set; }
        public string NombreTipo { get; set; }
        public List<RegistroLog>? Registros { get; set; }

    }
}
