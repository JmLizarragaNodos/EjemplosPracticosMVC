using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Web.Mvc;

// Es necesario instalar
// System.IdentityModel.Tokens.Jwt 5.5.0

namespace Web.Auth
{
    public class ValidarAutorizacion : AuthorizeAttribute
    {
        private readonly Permisos[] _roles;

        private readonly string secretKey = "tu_secreto_que_deberia_ser_mas_seguro";

        public ValidarAutorizacion(params Permisos[] roles)
        {
            _roles = roles;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            bool autorizado = false;

            if (ObtenerToken(filterContext, out string token))
            {
                var claimsPrincipal = ValidateToken(token);

                if (claimsPrincipal != null)
                {
                    autorizado = false;

                    var username = claimsPrincipal.Identity.Name;

                    // Obtener permisos del token
                    var claimsPermisos = claimsPrincipal.FindAll("Permiso").Select(c => c.Value).ToList();

                    List<Permisos> listaPermisosToken = claimsPermisos
                        .Select(p => Enum.TryParse(p, out Permisos permiso) ? permiso : (Permisos?)null)
                        .Where(p => p.HasValue)
                        .Select(p => p.Value)
                        .ToList();

                    foreach (Permisos rol in _roles)
                    {
                        if (listaPermisosToken.Contains(rol))
                        {
                            autorizado = true;
                            break;
                        }
                    }
                }
            }

            if (!autorizado)
            {
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;

                filterContext.Result = new JsonResult
                {
                    Data = new
                    {
                        status = 401,
                        errores = new List<string>() { "No autorizado" }
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

                return;
            }
        }


        public ClaimsPrincipal ValidateToken(string token)
        {
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false, // Cambiar según tus necesidades
                ValidateAudience = false, // Cambiar según tus necesidades
                ClockSkew = TimeSpan.Zero // Opcional: eliminar la tolerancia de 5 minutos por defecto
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return principal;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static bool ObtenerToken(AuthorizationContext filterContext, out string token)
        {
            token = string.Empty;
            var authorizationHeader = filterContext.HttpContext.Request.Headers["Authorization"];

            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
                token = authorizationHeader.Substring("Bearer ".Length).Trim();

            return !string.IsNullOrEmpty(token) && token != "null";
        }
    }

}