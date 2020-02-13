using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazyn.Models
{
    class Zamowienia
    {
        public int ID_Zamowienia { get; set; }
        public int ID_Klienta { get; set; }
        public int Rabat { get; set; }
        public string Nazwa_Klienta { get; set; }
        public IList<Opisy_zamowien> Opis {get; set;}
        public override string ToString()
        {
            string op = "";
            float priceAfterD = 0;
            foreach (Opisy_zamowien opis in Opis)
            {
                op += (opis.ToString()+ Environment.NewLine);
                priceAfterD += opis.Cena;
            }
            float rab = Rabat;
            float priceF = priceAfterD - priceAfterD * (rab / 100);
            return string.Format("Klient: {0,-12} Rabat [%]: {1,3}{2}Lista produktów:{3}{4}{5,-20}{6,5}{7,9}{8}{9}Cena po rabcie:{10,20}",
                Nazwa_Klienta, Rabat.ToString(), Environment.NewLine,Environment.NewLine , Environment.NewLine, "Nazwa:","Ilość:","Cena:",Environment.NewLine , op + Environment.NewLine, priceF.ToString());
        }
    }
}
