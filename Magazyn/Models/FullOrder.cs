using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazyn.Models
{
    class FullOrder
    {
        public int ID_Zamowienia { get; set; }
        public int ID_Klienta { get; set; }
        public int Rabat { get; set; }
        //public int ID_Produktu { get; set; }

        public List<Opisy_zamowien> OpisZamowienia { get; set; }

        
    }
}
