using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazyn.Models
{
    class Producenci
    {
        public int ID_Producenta { get; set; }
        public string Nazwa_Producenta { get; set; }
        public int ID_Kraju { get; set; }
        public override string ToString()
        {
            return Nazwa_Producenta;
        }
    }
}
