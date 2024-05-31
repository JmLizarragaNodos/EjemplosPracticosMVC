using System.Collections.Generic;

namespace Web.Models
{
    public class Region
    {
        public int idRegion { get; set; }
        public string nombreRegion { get; set; }
        public List<Provincia> provincias { get; set; }
    }
}