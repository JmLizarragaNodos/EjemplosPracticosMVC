using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Web.Models;

namespace Web.Helpers
{
    public static class ChileHelper
    {
        public static List<Region> ObtenerRegiones()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory").ToString(), "infoChile.json");

            if (!File.Exists(filePath)) 
                throw new Exception("No se encontró el archivo infoChile.json");
    
            string jsonContent = File.ReadAllText(filePath, Encoding.GetEncoding("ISO-8859-1"));
            return JsonConvert.DeserializeObject<List<Region>>(jsonContent); 
        }
    }
}