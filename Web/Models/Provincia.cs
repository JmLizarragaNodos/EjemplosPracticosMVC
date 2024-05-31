
using System.Collections.Generic;

namespace Web.Models
{
    public class Provincia
    {
        public int idProvincia { get; set; }
        public string nombreProvincia { get; set; }
        public List<Comuna> comunas { get; set; }
    }
}