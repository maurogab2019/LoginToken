namespace api_ejemplar.Models
{
    public class RespuestaLogin
    {
        public string Token { get; set; }
        public string respuesta { get; set; }
        public int codigo { get; set; }
        public bool estado { get; set; }
        public string nombreUsuario { get; set; }
    }
}
