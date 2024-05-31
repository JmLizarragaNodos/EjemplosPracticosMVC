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
    }
}