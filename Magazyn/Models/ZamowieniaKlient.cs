using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazyn.Models
{
    class ZamowieniaKlient
    {
        public int ID_Zamowienia { get; set; }
        public int ID_Klienta { get; set; }
        public int Rabat { get; set; }
        public string Nazwa_Klienta { get; set; }
        public IList<Opisy_zamowien> Opis { get; set; }
        public override string ToString()
        {
            return string.Format("Klient: {0} Rabat: {1} Opis:", Nazwa_Klienta, Rabat.ToString());
        }
    }
}
