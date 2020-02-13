using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazyn
{
    class Klienci
    {
        public int ID_Klienta { get; set; }
        public string Nazwa_Klienta { get; set; }
        public string Numer_Kontaktowy { get; set; }
        public string Adres { get; set; }

        public override string ToString()
        {
            return string.Format("{0,-30} {1,-20} {2,-30}", Nazwa_Klienta, Numer_Kontaktowy, Adres);
        }
    }

    
}
