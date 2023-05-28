using api_ejemplar.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace api_ejemplar.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        public readonly ApplicationDbContext db;
        public UsuarioController() 
        {
            db= new ApplicationDbContext();
        }
        // GET: api/<UsuarioController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(db.USUARIO.ToList());
        }

        // GET api/<UsuarioController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UsuarioController>
        [HttpPost]
        public IActionResult Post(USUARIO usuario)
        {
            try
            {
                if (usuario == null)
                {
                    var error = new ERRORES { fecha = DateTime.Now, MensajeError = "el usuario ingresado a registrar es nulo" };
                    db.ERRORES.Add(error);
                    db.SaveChangesAsync();
                    return StatusCode(505, error);
                }
                if ((usuario.nombreUsuario.Length > 30 || usuario.contrasena.Length > 10 || usuario.estado.Length > 10 || usuario.mail.Length > 50) || usuario.nombreUsuario == null || usuario.contrasena == null)
                {
                    var error = new ERRORES { fecha = DateTime.Now, MensajeError = "revise el largo de los datos ingreados, recorda completar campos obligatorios" };
                    db.ERRORES.Add(error);
                    db.SaveChangesAsync();
                    return StatusCode(505, error);
                }
                if (db.USUARIO.Where(u=>u.nombreUsuario==usuario.nombreUsuario).FirstOrDefault() != null)
                {
                    var error = new ERRORES { fecha = DateTime.Now, MensajeError = "nombre usuario ya existente" };
                    db.ERRORES.Add(error);
                    db.SaveChangesAsync();
                    return StatusCode(505, error);
                }
                //BCrypt.Net-Next ESTE INSTALAR EN NUGET
                usuario.contrasena = BCrypt.Net.BCrypt.HashPassword(usuario.contrasena);

                db.USUARIO.Add(usuario);
                db.SaveChanges();
                //bool contrasenaValida = BCrypt.Net.BCrypt.Verify(cotnraseñaPlana, "$2a$11$GCF8fqhN2vmhXw5CFhCHOEinKYj0z4LOiYmNA0etPxD8gPl185..");

                return Ok(new { resultado = usuario });
            }
            catch (Exception ex)
            {
                var error = new ERRORES { fecha = DateTime.Now, MensajeError = ex.Message };
                db.ERRORES.Add(error);
                db.SaveChanges();
                return StatusCode(505, error);
            }

        }

        [Route("api/login")]
        [HttpPost]
        public IActionResult postLogin(USUARIO usuario)
        {
            if (usuario == null)
            {
                var error = new ERRORES { fecha = DateTime.Now, MensajeError = "el usuario ingresado a registrar es nulo" };
                db.ERRORES.Add(error);
                db.SaveChangesAsync();
                return StatusCode(505, error);
            }

            var usuarioAloguar = db.USUARIO.Where(u => u.nombreUsuario == usuario.nombreUsuario).FirstOrDefault();
            if (usuarioAloguar == null)
            {
                var error = new ERRORES { fecha = DateTime.Now, MensajeError = "ocurrio un error al inicar sesion,intente de nuevo" };
                db.ERRORES.Add(error);
                db.SaveChangesAsync();
                return StatusCode(505, error);
            }

            var contrasenaEncriptada = usuarioAloguar.contrasena;
            bool contrasenaValida = BCrypt.Net.BCrypt.Verify(usuario.contrasena, contrasenaEncriptada);

            if (!contrasenaValida)
            {
                var error = new ERRORES { fecha = DateTime.Now, MensajeError = "contraseña o usuario incorrecto" };
                db.ERRORES.Add(error);
                db.SaveChangesAsync();
                return StatusCode(505, error);
            }

            return Ok(new { token = "toma el toekn", ok = "estas logueado gato" });


        }
        // PUT api/<UsuarioController>/5
        [HttpPut]
        public IActionResult putLogin(USUARIO usuario)
        {
            try
            {
                if (usuario == null)
                {
                    var error = new ERRORES { fecha = DateTime.Now, MensajeError = "el usuario ingresado a registrar es nulo" };
                    db.ERRORES.Add(error);
                    db.SaveChangesAsync();
                    return StatusCode(505, error);
                }

                var usuarioAloguar = db.USUARIO.Where(u => u.nombreUsuario == usuario.nombreUsuario).FirstOrDefault();
                if (usuarioAloguar == null)
                {
                    var error = new ERRORES { fecha = DateTime.Now, MensajeError = "ocurrio un error al inicar sesion,intente de nuevo" };
                    db.ERRORES.Add(error);
                    db.SaveChangesAsync();
                    return StatusCode(505, error);
                }

                var contrasenaEncriptada = usuarioAloguar.contrasena;
                bool contrasenaValida = BCrypt.Net.BCrypt.Verify(usuario.contrasena, contrasenaEncriptada);

                if (!contrasenaValida)
                {
                    var error = new ERRORES { fecha = DateTime.Now, MensajeError = "contraseña o usuario incorrecto" };
                    db.ERRORES.Add(error);
                    db.SaveChangesAsync();
                    return StatusCode(505, error);
                }


                var tokenDevolver = GenerateJwtToken(usuario);

                var logToken = new LOG_TOKEN() { fecha = DateTime.Now, token = tokenDevolver, usuario = usuarioAloguar.nombreUsuario };
                db.LOG_TOKEN.Add(logToken);
                db.SaveChanges();
                
                return Ok(new RespuestaLogin { nombreUsuario = usuarioAloguar.nombreUsuario, Token = tokenDevolver, respuesta = "estas logueado", codigo = 1, estado = true });
            }
            catch (Exception ex)
            {
                var error = new ERRORES { fecha = DateTime.Now, MensajeError = ex.Message };
                db.ERRORES.Add(error);
                db.SaveChanges();
                return StatusCode(505, error);
            }
            


        }


        private string GenerateJwtToken(USUARIO user)
        {
            // Configurar los claims del token
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.idUsuario.ToString()),
                new Claim(ClaimTypes.Name, user.nombreUsuario),
                new Claim(ClaimTypes.Email, user.mail),
                // Agrega otros claims según tus necesidades
            };

            // Generar la clave secreta
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("b5c3a9d1e8f0d2c4b9e1f8a0d2c4b9e1f8a0d2c4b9e1f8a0d2c4b9e1f8a0d2c")); // Reemplaza con tu clave secreta
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Configurar la descripción del token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7), // Configura la fecha de expiración del token
                SigningCredentials = credentials
            };

            // Generar el token JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Devolver el token como string
            return tokenHandler.WriteToken(token);
        }


        // DELETE api/<UsuarioController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
