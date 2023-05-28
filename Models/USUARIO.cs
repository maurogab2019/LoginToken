using System.ComponentModel.DataAnnotations;

namespace api_ejemplar.Models
{
    public class USUARIO
    {
        [Key]
        public int idUsuario { get; set; }
        public string nombreUsuario { get; set; }
        public string contrasena { get; set; }
        public string mail { get; set; }
        public string estado { get; set; }
    }
}
