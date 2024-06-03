using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Web.Dto;
using Web.Helpers;
using Web.Models;

namespace Web.Controllers
{
    public class Select2ComplejoController : Controller
    {
        private static List<Region> _regiones = ChileHelper.ObtenerRegiones();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ObtenerRegion(string search, int pageSize, int page)
        {
            var res = new RespuestaBackend();

            try
            {
                var query = _regiones.AsQueryable();

                if (!string.IsNullOrEmpty(search))
                    query = query.Where(d => d.nombreRegion.ToLower().Contains(search.ToLower()));

                int totalCount = query.Count();
                int offset = (page - 1) * pageSize;
                var dt = query.Skip(offset).Take(pageSize).ToList();

                bool traerMasRegistros = (dt.Count > 0 && dt.Count == pageSize);
                var retorno = new Select2(traerMasRegistros);

                foreach (Region x in dt)
                {
                    retorno.AgregarOption(x.idRegion.ToString(), x.nombreRegion);
                }

                res.objeto = retorno;
            }
            catch (Exception ex)
            {
                res.AgregarInternalServerError(ex.Message);
            }

            return Json(res);
        }

        [HttpPost]
        public ActionResult ObtenerProvincia(string search, int pageSize, int page, List<string> codigosRegiones)
        {
            var res = new RespuestaBackend();

            try
            {
                var provincias = _regiones
                .Where(region => codigosRegiones.Contains(region.idRegion.ToString()))
                .SelectMany(region => region.provincias);

                var query = provincias.AsQueryable(); 

                if (!string.IsNullOrEmpty(search))
                    query = query.Where(d => d.nombreProvincia.ToLower().Contains(search.ToLower()));

                int totalCount = query.Count();
                int offset = (page - 1) * pageSize;
                var dt = query.Skip(offset).Take(pageSize).ToList();

                bool traerMasRegistros = (dt.Count > 0 && dt.Count == pageSize);
                var retorno = new Select2(traerMasRegistros);

                foreach (Provincia x in dt)
                {
                    retorno.AgregarOption(x.idProvincia.ToString(), x.nombreProvincia);
                }

                res.objeto = retorno;
            }
            catch (Exception ex)
            {
                res.AgregarInternalServerError(ex.Message);
            }

            return Json(res);
        }

        [HttpPost]
        public ActionResult ObtenerComuna(string search, int pageSize, int page, List<string> codigosProvincias)
        {
            var res = new RespuestaBackend();

            try
            {
                var comunas = _regiones
                .SelectMany(region => region.provincias)
                .Where(provincia => codigosProvincias.Contains(provincia.idProvincia.ToString()))
                .SelectMany(provincia => provincia.comunas);

                var query = comunas.AsQueryable();

                if (!string.IsNullOrEmpty(search))
                    query = query.Where(d => d.nombreComuna.ToLower().Contains(search.ToLower()));

                int totalCount = query.Count();
                int offset = (page - 1) * pageSize;
                var dt = query.Skip(offset).Take(pageSize).ToList();

                bool traerMasRegistros = (dt.Count > 0 && dt.Count == pageSize);
                var retorno = new Select2(traerMasRegistros);

                foreach (Comuna x in dt)
                {
                    retorno.AgregarOption(x.idComuna.ToString(), x.nombreComuna);
                }

                res.objeto = retorno;
            }
            catch (Exception ex)
            {
                res.AgregarInternalServerError(ex.Message);
            }

            return Json(res);
        }

        public ActionResult TraerDatosMultiple()
        {
            var res = new RespuestaBackend();

            try
            {
                var regionesSeleccionadas = new HashSet<int> { 3, 6 };
                var provinciasSeleccionadas = new HashSet<int> { 6, 7, 14 };
                var comunasSeleccionadas = new HashSet<int> { 16, 20, 47, 51 };

                var regiones = new List<OptionSelect2>();
                var provincias = new List<OptionSelect2>();
                var comunas = new List<OptionSelect2>();

                foreach (var region in _regiones.Where(region => regionesSeleccionadas.Contains(region.idRegion)))
                {
                    regiones.Add(new OptionSelect2(region.idRegion.ToString(), region.nombreRegion));

                    foreach (var provincia in region.provincias.Where(provincia => provinciasSeleccionadas.Contains(provincia.idProvincia)))
                    {
                        provincias.Add(new OptionSelect2(provincia.idProvincia.ToString(), provincia.nombreProvincia));

                        foreach (var comuna in provincia.comunas)
                        {
                            if (comunasSeleccionadas.Contains(comuna.idComuna))
                                comunas.Add(new OptionSelect2(comuna.idComuna.ToString(), comuna.nombreComuna));  
                        }
                    }
                }

                res.objeto = new { regiones, provincias, comunas };
            }
            catch (Exception ex)
            {
                res.AgregarInternalServerError(ex.Message);
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}