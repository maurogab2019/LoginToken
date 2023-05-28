using System.ComponentModel.DataAnnotations;

namespace api_ejemplar.Models
{
    public class ERRORES
    {
        [Key]
        public int IdError { get; set; }
        public DateTime fecha { get; set; }
        public string MensajeError { get; set; }
    }
}
