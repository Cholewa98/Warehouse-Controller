using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazyn.Models
{
    class FullInfo
    {
        public string Nazwa_Produktu { get; set;}
        public string Nazwa_Producenta { get; set;}
        public DateTime Data { get; set; }
        public int Ilosc { get; set; }
        public float Cena { get; set; }

        public int ID_Towaru { get; set; }
        public int ID_Produktu { get; set; }
        public int ID_Producenta { get; set; }

        public override string ToString()
        {
            return String.Format("Nazwa produktu: {0}\n Nazwa producenta: {1}\n Ilosc w parti: {2}\n Wartosc jednostkowa: {3}\n Data: {4}", Nazwa_Produktu, Nazwa_Producenta, Ilosc, Cena, Data);
        }
    }
}
