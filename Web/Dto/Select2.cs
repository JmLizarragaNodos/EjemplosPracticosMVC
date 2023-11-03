using System.Collections.Generic;

namespace Web.Dto
{
    public class Select2
    {
        public List<OptionSelect2> resultados { get; set; }
        public bool traerMasRegistros { get; set; }
        public object informacionExtra { get; set; }

        public Select2(bool traerMasRegistros)
        {
            resultados = new List<OptionSelect2>();
            this.traerMasRegistros = traerMasRegistros;
        }

        public void AgregarOption(string id, string text)
        {
            resultados.Add(new OptionSelect2(id, text));
        }
    }

    public class OptionSelect2
    {
        public string id { get; set; }
        public string text { get; set; }

        public OptionSelect2(string id, string text)
        {
            this.id = id;
            this.text = text;
        }
    }
}