using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Web.Dto;

// https://localhost:44353/EjemploCheckbox

namespace Web.Controllers
{
    public class EjemploCheckboxController : Controller
    {
        private static List<Region> _regiones = CargarRegiones();

        public ActionResult Index()
        {
            return View();
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

        public ActionResult ObtenerProvincias(string i_regiones)
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

        public ActionResult ObtenerComunas(string i_provincias)
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

        private static List<Region> CargarRegiones()
        {
            string cadena = "[{\"idRegion\":1,\"nombreRegion\":\"Arica y Parinacota\",\"provincias\":[{\"idProvincia\":1,\"nombreProvincia\":\"Arica\",\"comunas\":[{\"idComuna\":1,\"nombreComuna\":\"Arica\"},{\"idComuna\":2,\"nombreComuna\":\"Camarones\"}]},{\"idProvincia\":2,\"nombreProvincia\":\"Parinacota\",\"comunas\":[{\"idComuna\":3,\"nombreComuna\":\"Putre\"},{\"idComuna\":4,\"nombreComuna\":\"General Lagos\"}]}]},{\"idRegion\":2,\"nombreRegion\":\"Tarapacá\",\"provincias\":[{\"idProvincia\":3,\"nombreProvincia\":\"Iquique\",\"comunas\":[{\"idComuna\":5,\"nombreComuna\":\"Iquique\"},{\"idComuna\":6,\"nombreComuna\":\"Alto Hospicio\"}]},{\"idProvincia\":4,\"nombreProvincia\":\"Tamarugal\",\"comunas\":[{\"idComuna\":7,\"nombreComuna\":\"Pozo Almonte\"},{\"idComuna\":8,\"nombreComuna\":\"Camiña\"},{\"idComuna\":9,\"nombreComuna\":\"Colchane\"},{\"idComuna\":10,\"nombreComuna\":\"Huara\"},{\"idComuna\":11,\"nombreComuna\":\"Pica\"}]}]},{\"idRegion\":3,\"nombreRegion\":\"Antofagasta\",\"provincias\":[{\"idProvincia\":5,\"nombreProvincia\":\"Antofagasta\",\"comunas\":[{\"idComuna\":12,\"nombreComuna\":\"Antofagasta\"},{\"idComuna\":13,\"nombreComuna\":\"Mejillones\"},{\"idComuna\":14,\"nombreComuna\":\"Sierra Gorda\"},{\"idComuna\":15,\"nombreComuna\":\"Taltal\"}]},{\"idProvincia\":6,\"nombreProvincia\":\"El Loa\",\"comunas\":[{\"idComuna\":16,\"nombreComuna\":\"Calama\"},{\"idComuna\":17,\"nombreComuna\":\"Ollagüe\"},{\"idComuna\":18,\"nombreComuna\":\"San Pedro de Atacama\"}]},{\"idProvincia\":7,\"nombreProvincia\":\"Tocopilla\",\"comunas\":[{\"idComuna\":19,\"nombreComuna\":\"Tocopilla\"},{\"idComuna\":20,\"nombreComuna\":\"María Elena\"}]}]},{\"idRegion\":4,\"nombreRegion\":\"Atacama\",\"provincias\":[{\"idProvincia\":8,\"nombreProvincia\":\"Copiapó\",\"comunas\":[{\"idComuna\":21,\"nombreComuna\":\"Copiapó\"},{\"idComuna\":22,\"nombreComuna\":\"Caldera\"},{\"idComuna\":23,\"nombreComuna\":\"Tierra Amarilla\"}]},{\"idProvincia\":9,\"nombreProvincia\":\"Chañaral\",\"comunas\":[{\"idComuna\":24,\"nombreComuna\":\"Chañaral\"},{\"idComuna\":25,\"nombreComuna\":\"Diego de Almagro\"}]},{\"idProvincia\":10,\"nombreProvincia\":\"Huasco\",\"comunas\":[{\"idComuna\":26,\"nombreComuna\":\"Vallenar\"},{\"idComuna\":27,\"nombreComuna\":\"Alto del Carmen\"},{\"idComuna\":28,\"nombreComuna\":\"Freirina\"},{\"idComuna\":29,\"nombreComuna\":\"Huasco\"}]}]},{\"idRegion\":5,\"nombreRegion\":\"Coquimbo\",\"provincias\":[{\"idProvincia\":11,\"nombreProvincia\":\"Elqui\",\"comunas\":[{\"idComuna\":30,\"nombreComuna\":\"La Serena\"},{\"idComuna\":31,\"nombreComuna\":\"Coquimbo\"},{\"idComuna\":32,\"nombreComuna\":\"Andacollo\"},{\"idComuna\":33,\"nombreComuna\":\"La Higuera\"},{\"idComuna\":34,\"nombreComuna\":\"Paiguano\"},{\"idComuna\":35,\"nombreComuna\":\"Vicuña\"}]},{\"idProvincia\":12,\"nombreProvincia\":\"Choapa\",\"comunas\":[{\"idComuna\":36,\"nombreComuna\":\"Illapel\"},{\"idComuna\":37,\"nombreComuna\":\"Canela\"},{\"idComuna\":38,\"nombreComuna\":\"Los Vilos\"},{\"idComuna\":39,\"nombreComuna\":\"Salamanca\"}]},{\"idProvincia\":13,\"nombreProvincia\":\"Limarí\",\"comunas\":[{\"idComuna\":40,\"nombreComuna\":\"Ovalle\"},{\"idComuna\":41,\"nombreComuna\":\"Combarbalá\"},{\"idComuna\":42,\"nombreComuna\":\"Monte Patria\"},{\"idComuna\":43,\"nombreComuna\":\"Punitaqui\"},{\"idComuna\":44,\"nombreComuna\":\"Río Hurtado\"}]}]},{\"idRegion\":6,\"nombreRegion\":\"Valparaíso\",\"provincias\":[{\"idProvincia\":14,\"nombreProvincia\":\"Valparaíso\",\"comunas\":[{\"idComuna\":45,\"nombreComuna\":\"Valparaíso\"},{\"idComuna\":46,\"nombreComuna\":\"Casablanca\"},{\"idComuna\":47,\"nombreComuna\":\"Concón\"},{\"idComuna\":48,\"nombreComuna\":\"Juan Fernández\"},{\"idComuna\":49,\"nombreComuna\":\"Puchuncaví\"},{\"idComuna\":50,\"nombreComuna\":\"Quintero\"},{\"idComuna\":51,\"nombreComuna\":\"Viña del Mar\"}]},{\"idProvincia\":15,\"nombreProvincia\":\"Isla de Pascua\",\"comunas\":[{\"idComuna\":52,\"nombreComuna\":\"Isla de Pascua\"}]},{\"idProvincia\":16,\"nombreProvincia\":\"Los Andes\",\"comunas\":[{\"idComuna\":53,\"nombreComuna\":\"Los Andes\"},{\"idComuna\":54,\"nombreComuna\":\"Calle Larga\"},{\"idComuna\":55,\"nombreComuna\":\"Riconada\"},{\"idComuna\":56,\"nombreComuna\":\"San Esteban\"}]},{\"idProvincia\":17,\"nombreProvincia\":\"Petorca\",\"comunas\":[{\"idComuna\":57,\"nombreComuna\":\"La Ligua\"},{\"idComuna\":58,\"nombreComuna\":\"Cabildo\"},{\"idComuna\":59,\"nombreComuna\":\"Papudo\"},{\"idComuna\":60,\"nombreComuna\":\"Petorca\"},{\"idComuna\":61,\"nombreComuna\":\"Zapallar\"}]},{\"idProvincia\":18,\"nombreProvincia\":\"Quillota\",\"comunas\":[{\"idComuna\":62,\"nombreComuna\":\"Quillota\"},{\"idComuna\":63,\"nombreComuna\":\"La Calera\"},{\"idComuna\":64,\"nombreComuna\":\"Hijuelas\"},{\"idComuna\":65,\"nombreComuna\":\"La Cruz\"},{\"idComuna\":66,\"nombreComuna\":\"Nogales\"}]},{\"idProvincia\":19,\"nombreProvincia\":\"San Antonio\",\"comunas\":[{\"idComuna\":67,\"nombreComuna\":\"San Antonio\"},{\"idComuna\":68,\"nombreComuna\":\"Algarrobo\"},{\"idComuna\":69,\"nombreComuna\":\"Cartagena\"},{\"idComuna\":70,\"nombreComuna\":\"El Quisco\"},{\"idComuna\":71,\"nombreComuna\":\"El Tabo\"},{\"idComuna\":72,\"nombreComuna\":\"Santo Domingo\"}]},{\"idProvincia\":20,\"nombreProvincia\":\"San Felipe de Aconcagua\",\"comunas\":[{\"idComuna\":73,\"nombreComuna\":\"San Felipe\"},{\"idComuna\":74,\"nombreComuna\":\"Catemu\"},{\"idComuna\":75,\"nombreComuna\":\"Llaillay\"},{\"idComuna\":76,\"nombreComuna\":\"Panquehue\"},{\"idComuna\":77,\"nombreComuna\":\"Putaendo\"},{\"idComuna\":78,\"nombreComuna\":\"Santa María\"}]},{\"idProvincia\":21,\"nombreProvincia\":\"Marga Marga\",\"comunas\":[{\"idComuna\":79,\"nombreComuna\":\"Quilpué\"},{\"idComuna\":80,\"nombreComuna\":\"Limache\"},{\"idComuna\":81,\"nombreComuna\":\"Olmué\"},{\"idComuna\":82,\"nombreComuna\":\"Villa Alemana\"}]}]},{\"idRegion\":7,\"nombreRegion\":\"Región del Libertador Gral. Bernardo O'Higgins\",\"provincias\":[{\"idProvincia\":22,\"nombreProvincia\":\"Cachapoal\",\"comunas\":[{\"idComuna\":83,\"nombreComuna\":\"Rancagua\"},{\"idComuna\":84,\"nombreComuna\":\"Codegua\"},{\"idComuna\":85,\"nombreComuna\":\"Coinco\"},{\"idComuna\":86,\"nombreComuna\":\"Coltauco\"},{\"idComuna\":87,\"nombreComuna\":\"Doñihue\"},{\"idComuna\":88,\"nombreComuna\":\"Graneros\"},{\"idComuna\":89,\"nombreComuna\":\"Las Cabras\"},{\"idComuna\":90,\"nombreComuna\":\"Machalí\"},{\"idComuna\":91,\"nombreComuna\":\"Malloa\"},{\"idComuna\":92,\"nombreComuna\":\"Mostazal\"},{\"idComuna\":93,\"nombreComuna\":\"Olivar\"},{\"idComuna\":94,\"nombreComuna\":\"Peumo\"},{\"idComuna\":95,\"nombreComuna\":\"Pichidegua\"},{\"idComuna\":96,\"nombreComuna\":\"Quinta de Tilcoco\"},{\"idComuna\":97,\"nombreComuna\":\"Rengo\"},{\"idComuna\":98,\"nombreComuna\":\"Requínoa\"},{\"idComuna\":99,\"nombreComuna\":\"San Vicente\"}]},{\"idProvincia\":23,\"nombreProvincia\":\"Cardenal Caro\",\"comunas\":[{\"idComuna\":100,\"nombreComuna\":\"Pichilemu\"},{\"idComuna\":101,\"nombreComuna\":\"La Estrella\"},{\"idComuna\":102,\"nombreComuna\":\"Litueche\"},{\"idComuna\":103,\"nombreComuna\":\"Marichihue\"},{\"idComuna\":104,\"nombreComuna\":\"Navidad\"},{\"idComuna\":105,\"nombreComuna\":\"Paredones\"}]},{\"idProvincia\":24,\"nombreProvincia\":\"Colchagua\",\"comunas\":[{\"idComuna\":106,\"nombreComuna\":\"San Fernando\"},{\"idComuna\":107,\"nombreComuna\":\"Chépica\"},{\"idComuna\":108,\"nombreComuna\":\"Chimbarongo\"},{\"idComuna\":109,\"nombreComuna\":\"Lolol\"},{\"idComuna\":110,\"nombreComuna\":\"Nancagua\"},{\"idComuna\":111,\"nombreComuna\":\"Palmilla\"},{\"idComuna\":112,\"nombreComuna\":\"Peralillo\"},{\"idComuna\":113,\"nombreComuna\":\"Placilla\"},{\"idComuna\":114,\"nombreComuna\":\"Pumanque\"},{\"idComuna\":115,\"nombreComuna\":\"Santa Cruz\"}]}]},{\"idRegion\":8,\"nombreRegion\":\"Región del Maule\",\"provincias\":[{\"idProvincia\":25,\"nombreProvincia\":\"Talca\",\"comunas\":[{\"idComuna\":116,\"nombreComuna\":\"Talca\"},{\"idComuna\":117,\"nombreComuna\":\"Constitución\"},{\"idComuna\":118,\"nombreComuna\":\"Curepto\"},{\"idComuna\":119,\"nombreComuna\":\"Empedrado\"},{\"idComuna\":120,\"nombreComuna\":\"Maule\"},{\"idComuna\":121,\"nombreComuna\":\"Pelarco\"},{\"idComuna\":122,\"nombreComuna\":\"Pencahue\"},{\"idComuna\":123,\"nombreComuna\":\"Río Claro\"},{\"idComuna\":124,\"nombreComuna\":\"San Clemente\"},{\"idComuna\":125,\"nombreComuna\":\"San Rafael\"}]},{\"idProvincia\":26,\"nombreProvincia\":\"Cauquenes\",\"comunas\":[{\"idComuna\":126,\"nombreComuna\":\"Cauquenes\"},{\"idComuna\":127,\"nombreComuna\":\"Chanco\"},{\"idComuna\":128,\"nombreComuna\":\"Pelluhue\"}]},{\"idProvincia\":27,\"nombreProvincia\":\"Curicó\",\"comunas\":[{\"idComuna\":129,\"nombreComuna\":\"Curicó\"},{\"idComuna\":130,\"nombreComuna\":\"Hualañé\"},{\"idComuna\":131,\"nombreComuna\":\"Licantén\"},{\"idComuna\":132,\"nombreComuna\":\"Molina\"},{\"idComuna\":133,\"nombreComuna\":\"Rauco\"},{\"idComuna\":134,\"nombreComuna\":\"Romeral\"},{\"idComuna\":135,\"nombreComuna\":\"Sagrada Familia\"},{\"idComuna\":136,\"nombreComuna\":\"Teno\"},{\"idComuna\":137,\"nombreComuna\":\"Vichuquén\"}]},{\"idProvincia\":28,\"nombreProvincia\":\"Linares\",\"comunas\":[{\"idComuna\":138,\"nombreComuna\":\"Linares\"},{\"idComuna\":139,\"nombreComuna\":\"Colbún\"},{\"idComuna\":140,\"nombreComuna\":\"Longaví\"},{\"idComuna\":141,\"nombreComuna\":\"Parral\"},{\"idComuna\":142,\"nombreComuna\":\"Retiro\"},{\"idComuna\":143,\"nombreComuna\":\"San Javier\"},{\"idComuna\":144,\"nombreComuna\":\"Villa Alegre\"},{\"idComuna\":145,\"nombreComuna\":\"Yerbas Buenas\"}]}]},{\"idRegion\":9,\"nombreRegion\":\"Región de Ñuble\",\"provincias\":[{\"idProvincia\":29,\"nombreProvincia\":\"Diguillín\",\"comunas\":[{\"idComuna\":146,\"nombreComuna\":\"Chillán\"},{\"idComuna\":147,\"nombreComuna\":\"Chillán Viejo\"},{\"idComuna\":148,\"nombreComuna\":\"Quillón\"},{\"idComuna\":149,\"nombreComuna\":\"Bulnes\"},{\"idComuna\":150,\"nombreComuna\":\"San Ignacio\"},{\"idComuna\":151,\"nombreComuna\":\"El Carmen\"},{\"idComuna\":152,\"nombreComuna\":\"Pinto\"},{\"idComuna\":153,\"nombreComuna\":\"Pemuco\"},{\"idComuna\":154,\"nombreComuna\":\"Yungay\"}]},{\"idProvincia\":30,\"nombreProvincia\":\"Itata\",\"comunas\":[{\"idComuna\":155,\"nombreComuna\":\"Quirihue\"},{\"idComuna\":156,\"nombreComuna\":\"Cobquecura\"},{\"idComuna\":157,\"nombreComuna\":\"Ninhue\"},{\"idComuna\":158,\"nombreComuna\":\"Treguaco\"},{\"idComuna\":159,\"nombreComuna\":\"Coelemu\"},{\"idComuna\":160,\"nombreComuna\":\"Portezuelo\"},{\"idComuna\":161,\"nombreComuna\":\"Ránqui\"}]},{\"idProvincia\":31,\"nombreProvincia\":\"Punilla\",\"comunas\":[{\"idComuna\":162,\"nombreComuna\":\"San Carlos\"},{\"idComuna\":163,\"nombreComuna\":\"Ñiquén\"},{\"idComuna\":164,\"nombreComuna\":\"Coihueco\"},{\"idComuna\":165,\"nombreComuna\":\"San Fabián\"},{\"idComuna\":166,\"nombreComuna\":\"San Nico\"}]}]},{\"idRegion\":10,\"nombreRegion\":\"Región del Biobío\",\"provincias\":[{\"idProvincia\":32,\"nombreProvincia\":\"Concepción\",\"comunas\":[{\"idComuna\":167,\"nombreComuna\":\"Concepción\"},{\"idComuna\":168,\"nombreComuna\":\"Coronel\"},{\"idComuna\":169,\"nombreComuna\":\"Chiguayante\"},{\"idComuna\":170,\"nombreComuna\":\"Florida\"},{\"idComuna\":171,\"nombreComuna\":\"Hualqui\"},{\"idComuna\":172,\"nombreComuna\":\"Lota\"},{\"idComuna\":173,\"nombreComuna\":\"Penco\"},{\"idComuna\":174,\"nombreComuna\":\"San Pedro de la Paz\"},{\"idComuna\":175,\"nombreComuna\":\"Santa Juana\"},{\"idComuna\":176,\"nombreComuna\":\"Talcahuano\"},{\"idComuna\":177,\"nombreComuna\":\"Tomé\"},{\"idComuna\":178,\"nombreComuna\":\"Hualpén\"}]},{\"idProvincia\":33,\"nombreProvincia\":\"Arauco\",\"comunas\":[{\"idComuna\":179,\"nombreComuna\":\"Lebu\"},{\"idComuna\":180,\"nombreComuna\":\"Arauco\"},{\"idComuna\":181,\"nombreComuna\":\"Cañete\"},{\"idComuna\":182,\"nombreComuna\":\"Contulmo\"},{\"idComuna\":183,\"nombreComuna\":\"Curanilahue\"},{\"idComuna\":184,\"nombreComuna\":\"Los Álamos\"},{\"idComuna\":185,\"nombreComuna\":\"Tirúa\"}]},{\"idProvincia\":34,\"nombreProvincia\":\"Biobío\",\"comunas\":[{\"idComuna\":186,\"nombreComuna\":\"Los Ángeles\"},{\"idComuna\":187,\"nombreComuna\":\"Antuco\"},{\"idComuna\":188,\"nombreComuna\":\"Cabrero\"},{\"idComuna\":189,\"nombreComuna\":\"Laja\"},{\"idComuna\":190,\"nombreComuna\":\"Mulchén\"},{\"idComuna\":191,\"nombreComuna\":\"Nacimiento\"},{\"idComuna\":192,\"nombreComuna\":\"Negrete\"},{\"idComuna\":193,\"nombreComuna\":\"Quilaco\"},{\"idComuna\":194,\"nombreComuna\":\"Quilleco\"},{\"idComuna\":195,\"nombreComuna\":\"San Rosendo\"},{\"idComuna\":196,\"nombreComuna\":\"Santa Bárbara\"},{\"idComuna\":197,\"nombreComuna\":\"Tucapel\"},{\"idComuna\":198,\"nombreComuna\":\"Yumbel\"},{\"idComuna\":199,\"nombreComuna\":\"Alto Biobío\"}]}]},{\"idRegion\":11,\"nombreRegion\":\"Región de la Araucanía\",\"provincias\":[{\"idProvincia\":35,\"nombreProvincia\":\"Cautín\",\"comunas\":[{\"idComuna\":200,\"nombreComuna\":\"Temuco\"},{\"idComuna\":201,\"nombreComuna\":\"Carahue\"},{\"idComuna\":202,\"nombreComuna\":\"Cunco\"},{\"idComuna\":203,\"nombreComuna\":\"Curarrehue\"},{\"idComuna\":204,\"nombreComuna\":\"Freire\"},{\"idComuna\":205,\"nombreComuna\":\"Galvarino\"},{\"idComuna\":206,\"nombreComuna\":\"Gorbea\"},{\"idComuna\":207,\"nombreComuna\":\"Lautaro\"},{\"idComuna\":208,\"nombreComuna\":\"Loncoche\"},{\"idComuna\":209,\"nombreComuna\":\"Melipeuco\"},{\"idComuna\":210,\"nombreComuna\":\"Nueva Imperial\"},{\"idComuna\":211,\"nombreComuna\":\"Padre Las Casas\"},{\"idComuna\":212,\"nombreComuna\":\"Perquenco\"},{\"idComuna\":213,\"nombreComuna\":\"Pitrufquén\"},{\"idComuna\":214,\"nombreComuna\":\"Pucón\"},{\"idComuna\":215,\"nombreComuna\":\"Saavedra\"},{\"idComuna\":216,\"nombreComuna\":\"Teodoro Schmidt\"},{\"idComuna\":217,\"nombreComuna\":\"Toltén\"},{\"idComuna\":218,\"nombreComuna\":\"Vilcún\"},{\"idComuna\":219,\"nombreComuna\":\"Villarrica\"},{\"idComuna\":220,\"nombreComuna\":\"Cholchol\"}]},{\"idProvincia\":36,\"nombreProvincia\":\"Malleco\",\"comunas\":[{\"idComuna\":221,\"nombreComuna\":\"Angol\"},{\"idComuna\":222,\"nombreComuna\":\"Collipulli\"},{\"idComuna\":223,\"nombreComuna\":\"Curacautín\"},{\"idComuna\":224,\"nombreComuna\":\"Ercilla\"},{\"idComuna\":225,\"nombreComuna\":\"Lonquimay\"},{\"idComuna\":226,\"nombreComuna\":\"Los Sauces\"},{\"idComuna\":227,\"nombreComuna\":\"Lumaco\"},{\"idComuna\":228,\"nombreComuna\":\"Purén\"},{\"idComuna\":229,\"nombreComuna\":\"Renaico\"},{\"idComuna\":230,\"nombreComuna\":\"Traiguén\"},{\"idComuna\":231,\"nombreComuna\":\"Victoria\"}]}]},{\"idRegion\":12,\"nombreRegion\":\"Región de los Ríos\",\"provincias\":[{\"idProvincia\":37,\"nombreProvincia\":\"Valdivia\",\"comunas\":[{\"idComuna\":232,\"nombreComuna\":\"Valdivia\"},{\"idComuna\":233,\"nombreComuna\":\"Corral\"},{\"idComuna\":234,\"nombreComuna\":\"Lanco\"},{\"idComuna\":235,\"nombreComuna\":\"Los Lagos\"},{\"idComuna\":236,\"nombreComuna\":\"Máfil\"},{\"idComuna\":237,\"nombreComuna\":\"Mariquina\"},{\"idComuna\":238,\"nombreComuna\":\"Paillaco\"},{\"idComuna\":239,\"nombreComuna\":\"Panguipulli\"}]},{\"idProvincia\":38,\"nombreProvincia\":\"Ranco\",\"comunas\":[{\"idComuna\":240,\"nombreComuna\":\"La Unión\"},{\"idComuna\":241,\"nombreComuna\":\"Futrono\"},{\"idComuna\":242,\"nombreComuna\":\"Lago Ranco\"},{\"idComuna\":243,\"nombreComuna\":\"Río Bueno\"}]}]},{\"idRegion\":13,\"nombreRegion\":\"Región de los Lagos\",\"provincias\":[{\"idProvincia\":39,\"nombreProvincia\":\"Llanquihue\",\"comunas\":[{\"idComuna\":244,\"nombreComuna\":\"Puerto Montt\"},{\"idComuna\":245,\"nombreComuna\":\"Calbuco\"},{\"idComuna\":246,\"nombreComuna\":\"Cochamó\"},{\"idComuna\":247,\"nombreComuna\":\"Fresia\"},{\"idComuna\":248,\"nombreComuna\":\"Frutillar\"},{\"idComuna\":249,\"nombreComuna\":\"Los Muermos\"},{\"idComuna\":250,\"nombreComuna\":\"Llanquihue\"},{\"idComuna\":251,\"nombreComuna\":\"Maullín\"},{\"idComuna\":252,\"nombreComuna\":\"Puerto Varas\"}]},{\"idProvincia\":40,\"nombreProvincia\":\"Chiloé\",\"comunas\":[{\"idComuna\":253,\"nombreComuna\":\"Castro\"},{\"idComuna\":254,\"nombreComuna\":\"Ancud\"},{\"idComuna\":255,\"nombreComuna\":\"Chonchi\"},{\"idComuna\":256,\"nombreComuna\":\"Curaco de Vélez\"},{\"idComuna\":257,\"nombreComuna\":\"Dalcahue\"},{\"idComuna\":258,\"nombreComuna\":\"Puqueldón\"},{\"idComuna\":259,\"nombreComuna\":\"Queilén\"},{\"idComuna\":260,\"nombreComuna\":\"Quellón\"},{\"idComuna\":261,\"nombreComuna\":\"Quemchi\"},{\"idComuna\":262,\"nombreComuna\":\"Quinchao\"}]},{\"idProvincia\":41,\"nombreProvincia\":\"Osorno\",\"comunas\":[{\"idComuna\":263,\"nombreComuna\":\"Osorno\"},{\"idComuna\":264,\"nombreComuna\":\"Puerto Octay\"},{\"idComuna\":265,\"nombreComuna\":\"Purranque\"},{\"idComuna\":266,\"nombreComuna\":\"Puyehue\"},{\"idComuna\":267,\"nombreComuna\":\"Río Negro\"},{\"idComuna\":268,\"nombreComuna\":\"San Juan de la Costa\"},{\"idComuna\":269,\"nombreComuna\":\"San Pablo\"}]},{\"idProvincia\":42,\"nombreProvincia\":\"Palena\",\"comunas\":[{\"idComuna\":270,\"nombreComuna\":\"Chaitén\"},{\"idComuna\":271,\"nombreComuna\":\"Futaleufú\"},{\"idComuna\":272,\"nombreComuna\":\"Hualaihué\"},{\"idComuna\":273,\"nombreComuna\":\"Palena\"}]}]},{\"idRegion\":14,\"nombreRegion\":\"Región Aisén del Gral. Carlos Ibañez del Campo\",\"provincias\":[{\"idProvincia\":43,\"nombreProvincia\":\"Coyhaique\",\"comunas\":[{\"idComuna\":274,\"nombreComuna\":\"Coihaique\"},{\"idComuna\":275,\"nombreComuna\":\"Lago Verde\"}]},{\"idProvincia\":44,\"nombreProvincia\":\"Aisén\",\"comunas\":[{\"idComuna\":276,\"nombreComuna\":\"Aisén\"},{\"idComuna\":277,\"nombreComuna\":\"Cisnes\"},{\"idComuna\":278,\"nombreComuna\":\"Guaitecas\"}]},{\"idProvincia\":45,\"nombreProvincia\":\"Capitán Pratt\",\"comunas\":[{\"idComuna\":279,\"nombreComuna\":\"Cochrane\"},{\"idComuna\":280,\"nombreComuna\":\"O'Higgins\"},{\"idComuna\":281,\"nombreComuna\":\"Tortel\"}]},{\"idProvincia\":46,\"nombreProvincia\":\"General Carrera\",\"comunas\":[{\"idComuna\":282,\"nombreComuna\":\"Chile Chico\"},{\"idComuna\":283,\"nombreComuna\":\"Río Ibáñez\"}]}]},{\"idRegion\":15,\"nombreRegion\":\"Región de Magallanes y de la Antártica Chilena\",\"provincias\":[{\"idProvincia\":47,\"nombreProvincia\":\"Magallanes\",\"comunas\":[{\"idComuna\":284,\"nombreComuna\":\"Punta Arenas\"},{\"idComuna\":285,\"nombreComuna\":\"Laguna Blanca\"},{\"idComuna\":286,\"nombreComuna\":\"Río Verde\"},{\"idComuna\":287,\"nombreComuna\":\"San Gregorio\"}]},{\"idProvincia\":48,\"nombreProvincia\":\"Antártica Chilena\",\"comunas\":[{\"idComuna\":288,\"nombreComuna\":\"Cabo de Hornos\"},{\"idComuna\":289,\"nombreComuna\":\"Antártica\"}]},{\"idProvincia\":49,\"nombreProvincia\":\"Tierra del Fuego\",\"comunas\":[{\"idComuna\":290,\"nombreComuna\":\"Porvenir\"},{\"idComuna\":291,\"nombreComuna\":\"Primavera\"},{\"idComuna\":292,\"nombreComuna\":\"Timaukel\"}]},{\"idProvincia\":50,\"nombreProvincia\":\"Última Esperanza\",\"comunas\":[{\"idComuna\":293,\"nombreComuna\":\"Natales\"},{\"idComuna\":294,\"nombreComuna\":\"Torres del Paine\"}]}]},{\"idRegion\":16,\"nombreRegion\":\"Región Metropolitana de Santiago\",\"provincias\":[{\"idProvincia\":51,\"nombreProvincia\":\"Santiago\",\"comunas\":[{\"idComuna\":295,\"nombreComuna\":\"Santiago\"},{\"idComuna\":296,\"nombreComuna\":\"Cerrillos\"},{\"idComuna\":297,\"nombreComuna\":\"Cerro Navia\"},{\"idComuna\":298,\"nombreComuna\":\"Conchalí\"},{\"idComuna\":299,\"nombreComuna\":\"El Bosque\"},{\"idComuna\":300,\"nombreComuna\":\"Estación Central\"},{\"idComuna\":301,\"nombreComuna\":\"Huechuraba\"},{\"idComuna\":302,\"nombreComuna\":\"Independencia\"},{\"idComuna\":303,\"nombreComuna\":\"La Cisterna\"},{\"idComuna\":304,\"nombreComuna\":\"La Florida\"},{\"idComuna\":305,\"nombreComuna\":\"La Granja\"},{\"idComuna\":306,\"nombreComuna\":\"La Pintana\"},{\"idComuna\":307,\"nombreComuna\":\"La Reina\"},{\"idComuna\":308,\"nombreComuna\":\"Las Condes\"},{\"idComuna\":309,\"nombreComuna\":\"Lo Barnechea\"},{\"idComuna\":310,\"nombreComuna\":\"Lo Espejo\"},{\"idComuna\":311,\"nombreComuna\":\"Lo Prado\"},{\"idComuna\":312,\"nombreComuna\":\"Macul\"},{\"idComuna\":313,\"nombreComuna\":\"Maipú\"},{\"idComuna\":314,\"nombreComuna\":\"Ñuñoa\"},{\"idComuna\":315,\"nombreComuna\":\"Pedro Aguirre Cerda\"},{\"idComuna\":316,\"nombreComuna\":\"Peñalolén\"},{\"idComuna\":317,\"nombreComuna\":\"Providencia\"},{\"idComuna\":318,\"nombreComuna\":\"Pudahuel\"},{\"idComuna\":319,\"nombreComuna\":\"Quilicura\"},{\"idComuna\":320,\"nombreComuna\":\"Quinta Normal\"},{\"idComuna\":321,\"nombreComuna\":\"Recoleta\"},{\"idComuna\":322,\"nombreComuna\":\"Renca\"},{\"idComuna\":323,\"nombreComuna\":\"San Joaquín\"},{\"idComuna\":324,\"nombreComuna\":\"San Miguel\"},{\"idComuna\":325,\"nombreComuna\":\"San Ramón\"},{\"idComuna\":326,\"nombreComuna\":\"Vitacura\"}]},{\"idProvincia\":52,\"nombreProvincia\":\"Cordillera\",\"comunas\":[{\"idComuna\":327,\"nombreComuna\":\"Puente Alto\"},{\"idComuna\":328,\"nombreComuna\":\"Pirque\"},{\"idComuna\":329,\"nombreComuna\":\"San José de Maipo\"}]},{\"idProvincia\":53,\"nombreProvincia\":\"Chacabuco\",\"comunas\":[{\"idComuna\":330,\"nombreComuna\":\"Colina\"},{\"idComuna\":331,\"nombreComuna\":\"Lampa\"},{\"idComuna\":332,\"nombreComuna\":\"Tiltil\"}]},{\"idProvincia\":54,\"nombreProvincia\":\"Maipo\",\"comunas\":[{\"idComuna\":333,\"nombreComuna\":\"San Bernardo\"},{\"idComuna\":334,\"nombreComuna\":\"Buin\"},{\"idComuna\":335,\"nombreComuna\":\"Calera de Tango\"},{\"idComuna\":336,\"nombreComuna\":\"Paine\"}]},{\"idProvincia\":55,\"nombreProvincia\":\"Mellipilla\",\"comunas\":[{\"idComuna\":337,\"nombreComuna\":\"Melipilla\"},{\"idComuna\":338,\"nombreComuna\":\"Alhué\"},{\"idComuna\":339,\"nombreComuna\":\"Curacaví\"},{\"idComuna\":340,\"nombreComuna\":\"María Pinto\"},{\"idComuna\":341,\"nombreComuna\":\"San Pedro\"}]},{\"idProvincia\":56,\"nombreProvincia\":\"Talagante\",\"comunas\":[{\"idComuna\":342,\"nombreComuna\":\"Talagante\"},{\"idComuna\":343,\"nombreComuna\":\"El Monte\"},{\"idComuna\":344,\"nombreComuna\":\"Isla de Maipo\"},{\"idComuna\":345,\"nombreComuna\":\"Padre Hurtado\"},{\"idComuna\":346,\"nombreComuna\":\"Peñaflor\"}]}]}]";
            return JsonConvert.DeserializeObject<List<Region>>(cadena);
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

        public class Region
        {
            public int idRegion { get; set; }
            public string nombreRegion { get; set; }
            public List<Provincia> provincias { get; set; }
        }

        public class Provincia
        {
            public int idProvincia { get; set; }
            public string nombreProvincia { get; set; }
            public List<Comuna> comunas { get; set; }
        }

        public class Comuna
        {
            public int idComuna { get; set; }
            public string nombreComuna { get; set; }
        }
    }
}