using System;

namespace Web.Helpers
{
    public static class TextosHelper
    {
        public static string ObtenerCadenaDeTexto(this int value)
        {
            string retorno = NumberToText(value);
            return retorno;
        }

        public static string NumberToText(long numero)
        {
            /*
            string a = NumberToText(1);             // uno
            string b = NumberToText(11);            // once
            string c = NumberToText(111);           // ciento once
            string d = NumberToText(1111);          // mil ciento once
            string e = NumberToText(11111);         // once mil ciento once
            string f = NumberToText(111111);        // ciento once mil ciento once
            string g = NumberToText(1111111);       // un millón ciento once mil ciento once
            string h = NumberToText(11111111);      // once millones ciento once mil ciento once
            string i = NumberToText(111111111);     // ciento once millones ciento once mil ciento once
            string j = NumberToText(1111111111);    // mil ciento once millones ciento once mil ciento once 
            */

            string Num2Text = "";
            if (numero < 0) return "menos " + NumberToText(Math.Abs(numero));

            if (numero == 0) Num2Text = "cero";
            else if (numero == 1) Num2Text = "uno";
            else if (numero == 2) Num2Text = "dos";
            else if (numero == 3) Num2Text = "tres";
            else if (numero == 4) Num2Text = "cuatro";
            else if (numero == 5) Num2Text = "cinco";
            else if (numero == 6) Num2Text = "seis";
            else if (numero == 7) Num2Text = "siete";
            else if (numero == 8) Num2Text = "ocho";
            else if (numero == 9) Num2Text = "nueve";
            else if (numero == 10) Num2Text = "diez";
            else if (numero == 11) Num2Text = "once";
            else if (numero == 12) Num2Text = "doce";
            else if (numero == 13) Num2Text = "trece";
            else if (numero == 14) Num2Text = "catorce";
            else if (numero == 15) Num2Text = "quince";
            else if (numero < 20) Num2Text = "dieci" + NumberToText(numero - 10);
            else if (numero == 20) Num2Text = "veinte";
            else if (numero < 30) Num2Text = "veinti" + NumberToText(numero - 20);
            else if (numero == 30) Num2Text = "treinta";
            else if (numero == 40) Num2Text = "cuarenta";
            else if (numero == 50) Num2Text = "cincuenta";
            else if (numero == 60) Num2Text = "sesenta";
            else if (numero == 70) Num2Text = "setenta";
            else if (numero == 80) Num2Text = "ochenta";
            else if (numero == 90) Num2Text = "noventa";
            else if (numero < 100)
            {
                long u = numero % 10;
                Num2Text = string.Format("{0} y {1}", NumberToText((numero / 10) * 10), (u == 1 ? "un" : NumberToText(numero % 10)));
            }
            else if (numero == 100) Num2Text = "cien";
            else if (numero < 200) Num2Text = "ciento " + NumberToText(numero - 100);
            else if ((numero == 200) || (numero == 300) || (numero == 400) || (numero == 600) || (numero == 800))
                Num2Text = NumberToText((numero / 100)) + "cientos";
            else if (numero == 500) Num2Text = "quinientos";
            else if (numero == 700) Num2Text = "setecientos";
            else if (numero == 900) Num2Text = "novecientos";
            else if (numero < 1000) Num2Text = string.Format("{0} {1}", NumberToText((numero / 100) * 100), NumberToText(numero % 100));
            else if (numero == 1000) Num2Text = "mil";
            else if (numero < 2000) Num2Text = "mil " + NumberToText(numero % 1000);
            else if (numero < 1000000)
            {
                Num2Text = NumberToText((numero / 1000)) + " mil";
                if ((numero % 1000) > 0) Num2Text += " " + NumberToText(numero % 1000);
            }
            else if (numero == 1000000) Num2Text = "un millón";
            else if (numero < 2000000) Num2Text = "un millón " + NumberToText(numero % 1000000);
            else if (numero < int.MaxValue)
            {
                Num2Text = NumberToText((numero / 1000000)) + " millones";
                if ((numero - (numero / 1000000) * 1000000) > 0) Num2Text += " " + NumberToText(numero - (numero / 1000000) * 1000000);
            }
            return Num2Text;
        }
    }
}