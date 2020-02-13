using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazyn.Models
{
    class Result
    {
        public int Ilosc { get; set; }
        public float Cena { get; set; }
        public int ID_Produktu { get; set; }
        public string Nazwa_Produktu { get; set; }
        public int ID_Towaru { get; set; }

        public override string ToString()
        {
            return string.Format("{0,-30} {1,15} {2,15}", Nazwa_Produktu.Trim(), Ilosc.ToString(), Cena.ToString());
        }
    }
    
}
