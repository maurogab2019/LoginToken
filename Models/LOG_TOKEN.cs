using System.ComponentModel.DataAnnotations;

namespace api_ejemplar.Models
{
    public class LOG_TOKEN
    {
        [Key]
        public int id { get; set; }
        public string usuario { get; set; }
        public DateTime fecha { get; set; }      
        public string token { get; set; }
    }
}
