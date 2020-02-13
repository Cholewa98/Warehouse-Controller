using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazyn.Models
{
    class Towary
    {
        public int ID_Produktu { get; set; }
        public DateTime Data { get; set; }
        public int Ilosc { get; set; }
        public float Cena { get; set; }
        public int ID_Towaru { get; set; }
        //public Produkt Produkt { get; set; }
    }
}
