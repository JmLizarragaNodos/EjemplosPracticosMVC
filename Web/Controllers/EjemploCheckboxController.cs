using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Web.Dto;
using Web.Helpers;
using Web.Models;

namespace Web.Controllers
{
    public class EjemploCheckboxController : Controller
    {
        private static List<Region> _regiones = ChileHelper.ObtenerRegiones();

        public ActionResult Index()  // https://localhost:44353/EjemploCheckbox
        {
            // string jsonString = JsonConvert.SerializeObject(_regiones, Formatting.Indented);

            return View();
        }

        public ActionResult Probando()  // https://localhost:44353/EjemploCheckbox/Probando
        {
            var res = new RespuestaBackend();

            try
            {
                var regiones = ObtenerRegionesDePrueba();
                regiones.FirstOrDefault(x => x.codigo == "3").estaSeleccionado = true;
                regiones.FirstOrDefault(x => x.codigo == "6").estaSeleccionado = true;

                var codigosRegiones = regiones.Where(x => x.estaSeleccionado).Select(x => x.codigo).ToList();

                var provincias = ObtenerProvinciasDePrueba(codigosRegiones);
                provincias.FirstOrDefault(x => x.codigo == "6").estaSeleccionado = true;
                provincias.FirstOrDefault(x => x.codigo == "7").estaSeleccionado = true;
                provincias.FirstOrDefault(x => x.codigo == "14").estaSeleccionado = true;

                var codigosProvincias = provincias.Where(x => x.estaSeleccionado).Select(x => x.codigo).ToList();

                var comunas = ObtenerComunasDePrueba(codigosProvincias);
                comunas.FirstOrDefault(x => x.codigo == "16").estaSeleccionado = true;
                comunas.FirstOrDefault(x => x.codigo == "20").estaSeleccionado = true;
                comunas.FirstOrDefault(x => x.codigo == "47").estaSeleccionado = true;
                comunas.FirstOrDefault(x => x.codigo == "51").estaSeleccionado = true;

                res.objeto = new { regiones, provincias, comunas };
            }
            catch (Exception ex)
            {
                res.AgregarInternalServerError(ex.Message);
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ObtenerRegiones()
        {
            var res = new RespuestaBackend();

            try
            {
                res.objeto = ObtenerRegionesDePrueba();
            }
            catch (Exception ex)
            {
                res.AgregarInternalServerError(ex.Message);
            }

            return Json(res);
        }

        public ActionResult ObtenerProvincias(string i_regiones)  // i_regiones="3,6"
        {
            var res = new RespuestaBackend();

            try
            {
                List<string> codigosRegiones = new List<string>(i_regiones.Split(','));
                List<ElementoCheckbox> provincias = ObtenerProvinciasDePrueba(codigosRegiones);
                res.objeto = provincias;
            }
            catch (Exception ex)
            {
                res.AgregarInternalServerError(ex.Message);
            }

            return Json(res);
        }

        public ActionResult ObtenerComunas(string i_provincias)  // i_provincias="6,7,14"
        {
            var res = new RespuestaBackend();

            try
            {
                List<string> codigosProvincias = new List<string>(i_provincias.Split(','));
                List<ElementoCheckbox> comunas = ObtenerComunasDePrueba(codigosProvincias);
                res.objeto = comunas;
            }
            catch (Exception ex)
            {
                res.AgregarInternalServerError(ex.Message);
            }

            return Json(res);
        }

        private List<ElementoCheckbox> ObtenerRegionesDePrueba()
        {
            return _regiones.Select(x => new ElementoCheckbox
            {
                codigo = x.idRegion.ToString(),
                descripcion = x.nombreRegion,
                estaSeleccionado = false
            }).ToList();
        }

        private List<ElementoCheckbox> ObtenerProvinciasDePrueba(List<string> codigosRegiones)
        {
            List<ElementoCheckbox> provincias = _regiones
                .Where(region => codigosRegiones.Contains(region.idRegion.ToString()))
                .SelectMany(region => region.provincias)
                .Select(provincia => new ElementoCheckbox
                {
                    codigo = provincia.idProvincia.ToString(),
                    descripcion = provincia.nombreProvincia,
                    estaSeleccionado = false
                })
                .ToList();

            return provincias;
        }

        private List<ElementoCheckbox> ObtenerComunasDePrueba(List<string> codigosProvincias)
        {
            List<ElementoCheckbox> comunas = _regiones
                .SelectMany(region => region.provincias)
                .Where(provincia => codigosProvincias.Contains(provincia.idProvincia.ToString()))
                .SelectMany(provincia => provincia.comunas)
                .Select(provincia => new ElementoCheckbox
                {
                    codigo = provincia.idComuna.ToString(),
                    descripcion = provincia.nombreComuna,
                    estaSeleccionado = false
                })
                    .ToList();

            return comunas;
        }

        public class ElementoCheckbox
        {
            public string codigo { get; set; }
            public string descripcion { get; set; }
            public bool estaSeleccionado { get; set; }
        }
    }
}