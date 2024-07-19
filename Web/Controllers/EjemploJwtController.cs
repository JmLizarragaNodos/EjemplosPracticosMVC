using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web.Mvc;
using Web.Auth;
using Web.Dto;

namespace Web.Controllers
{
    public class EjemploJwtController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var res = new RespuestaBackend();

            try
            {
                if (username == "jose" && password == "1234")
                {
                    List<Permisos> listaPermisos = new List<Permisos>() { Permisos.SELECT, Permisos.UPDATE };
                    //List<Permisos> listaPermisos = new List<Permisos>() { Permisos.SELECT };

                    var tokenString = GenerarJwt(username, listaPermisos);

                    res.objeto = tokenString;
                }
                else
                {
                    res.AgregarBadRequest("Username o Password incorrectos");
                }
            }
            catch (Exception ex)
            {
                res.AgregarInternalServerError(ex.Message);
            }

            return Json(res);
        }

        //[ValidarAutorizacion(Permisos.SELECT, Permisos.UPDATE)]
        [ValidarAutorizacion(Permisos.UPDATE)]
        [HttpPost]
        public ActionResult Probando(string texto)
        {
            var res = new RespuestaBackend();

            try
            {
                res.objeto = new { hola = true };
            }
            catch (Exception ex)
            {
                res.AgregarInternalServerError(ex.Message);
            }

            return Json(res);
        }

        private string GenerarJwt(string username, List<Permisos> listaPermisos)
        {
            var secretKey = "tu_secreto_que_deberia_ser_mas_seguro";
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username)
            };

            claims.AddRange(listaPermisos.Select(p => new Claim("Permiso", p.ToString())));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}