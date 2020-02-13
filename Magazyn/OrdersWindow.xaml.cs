using Dapper;
using Magazyn.Infrastructure;
using Magazyn.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Magazyn
{

    public partial class OrdersWindow : Window
    {
        public void ListBoxProductsRef()
        {
            listboxProducts.Items.Clear();

            var sql3 = "SELECT Z.*,K.*, O.*, P.* FROM Zamowienia Z INNER JOIN Klienci K ON Z.ID_Klienta=K.ID_Klienta " +
                "INNER JOIN Opisy_zamowien O ON Z.ID_Zamowienia = O.ID_Zamowienia " +
                "INNER JOIN Produkt P ON O.ID_Produktu = P.ID_Produktu";
            var lookbook = new Dictionary<int, Zamowienia>();

            _sqlConnection.Query<Zamowienia, Opisy_zamowien, Zamowienia>(sql3, (Z, O) => {
                Zamowienia zamowienia;
                if (!lookbook.TryGetValue(Z.ID_Zamowienia, out zamowienia))
                {
                    lookbook.Add(Z.ID_Zamowienia, zamowienia = Z);
                }
                if (zamowienia.Opis == null)
                {
                    zamowienia.Opis = new List<Opisy_zamowien>();
                }
                zamowienia.Opis.Add(O);
                return zamowienia;

            }, splitOn: "ID_Zamowienia").AsQueryable();
            var resultList = lookbook.Values;


            foreach (var item in lookbook.Values)
            {

                listboxProducts.Items.Add(item);
                Separator sep = new Separator();
                sep.Height = 2;
                listboxProducts.Items.Add(sep);
                //listboxProducts.Items.Add("-------------------------");
            }
        }

        private readonly IDbConnection _sqlConnection = SqlConnectionFactory.GetConnection();
        private List<Opisy_zamowien> ordersList = new List<Opisy_zamowien>();
        public int progressID = 0;

        private void ListBoxRefresh(List<Opisy_zamowien> orderList)
        {
            listboxOrders.Items.Clear();
            foreach (Opisy_zamowien opis in orderList)
            {
                listboxOrders.Items.Add(opis);
            }
        }


        public OrdersWindow()
        {
            ordersList.Clear();
            InitializeComponent();

            label.Content = string.Format("{0,-20} {1,5} {2,8}", "Nazwa:", "Ilość:", "Cena:");
            var sqll = "SELECT Z.*, K.* FROM Zamowienia Z INNER JOIN Klienci K ON Z.ID_Klienta = K.ID_Klienta";
            Zamowienia[] zam = _sqlConnection.Query<Zamowienia>(sqll).ToArray();

            

            var sql = "SELECT DISTINCT Produkt.ID_Produktu, Nazwa_Produktu FROM Produkt RIGHT JOIN Towary ON Produkt.ID_Produktu = Towary.ID_Produktu";

            Produkt[] produkts = _sqlConnection.Query<Produkt>(sql).ToArray();

            foreach (Produkt produkt in produkts)
            {
                comboProducts.Items.Add(produkt);
            }

            sql = "SELECT * FROM Klienci";
            Klienci[] kliencis = _sqlConnection.Query<Klienci>(sql).ToArray();

            foreach (Klienci klient in kliencis)
            {
                comboClients.Items.Add(klient);
            }
            comboProducts.SelectedIndex = 0;
            comboClients.SelectedIndex = 0;
            ListBoxProductsRef();
        }

        void windowClose(object sender, EventArgs e)
        {
            //foreach(Opisy_zamowien opi)
        }
        private void btnAddProduct(object sender, RoutedEventArgs e)
        {
            Produkt product = comboProducts.SelectedItem as Produkt;
            var productID = product.ID_Produktu;

            var sql = "SELECT P.ID_Produktu, ID_Towaru, Nazwa_Produktu, Data, Ilosc, Cena FROM Produkt P INNER JOIN Towary T ON P.ID_Produktu = T.ID_Produktu WHERE T.ID_Produktu=@product ORDER BY Data";
            FullInfo[] fullInfos = _sqlConnection.Query<FullInfo>(sql, new { product = productID }).ToArray();
            float finallPrice = 0;

            var sqlChceck = "SELECT SUM(Ilosc) FROM Towary T WHERE ID_Produktu =@id_p";
            int suma = _sqlConnection.Query<int>(sqlChceck, new { id_p = productID }).SingleOrDefault();

            errorQuan.Content = "";
            errorEmpty.Content = "";
            int res;
            if (String.IsNullOrEmpty(txtQuantity.Text))
            {
                errorQuan.Content += ("Pole ilosc nie może być"+Environment.NewLine+"puste!");

            }

            else if (!int.TryParse(txtQuantity.Text.Trim(), out res))
            {
                errorQuan.Content += "Pole ilosc musi być" + Environment.NewLine +"liczbą nieujamną!";


            }

            else if (int.TryParse(txtQuantity.Text.Trim(), out res) && int.Parse(txtQuantity.Text.Trim()) <= 0)
            {
                errorQuan.Content += "Pole ilosc nie może być" + Environment.NewLine +"ujemne!";
            }

            else if (suma < Int32.Parse(txtQuantity.Text))
            {
                errorEmpty.Content += "Brak wystarczającej ilosci" + Environment.NewLine +"prodoktów!";
            }
            else
            {
                string name = fullInfos[0].Nazwa_Produktu;
                int quantity = Int32.Parse(txtQuantity.Text);
                foreach (FullInfo full in fullInfos)
                {
                    if (quantity != 0)
                    {
                        if (full.Ilosc > quantity)
                        {
                            var sqlU = "UPDATE Towary SET Ilosc = @ilosc WHERE ID_Towaru = @idT";
                            finallPrice = quantity * full.Cena;
                            int ammount = full.Ilosc - quantity;
                            string ammountS = ammount.ToString();
                            string xd = full.ID_Towaru.ToString();
                            _sqlConnection.Execute(sqlU, new { ilosc = ammountS, idT = full.ID_Towaru.ToString() });

                            break;
                        }

                        else if (full.Ilosc <= quantity)
                        {
                            var sqlD = "DELETE FROM Towary WHERE ID_Towaru=@idT";
                            finallPrice += full.Ilosc * full.Cena;
                            quantity -= full.Ilosc;
                            _sqlConnection.Execute(sqlD, new { idT = full.ID_Towaru });

                        }
                    }
                }
                Opisy_zamowien opis = new Opisy_zamowien();

                opis.Cena = finallPrice;
                opis.ID_Produktu = productID;
                opis.Ilosc = quantity;
                opis.Nazwa_Produktu = name;
                var sqlGetLastID = "SELECT TOP 1 ID_Zamowienia from Zamowienia ORDER BY ID_Zamowienia DESC";


                Zamowienia lastID = _sqlConnection.Query<Zamowienia>(sqlGetLastID).SingleOrDefault();

                var sqlI = "INSERT INTO Opisy_zamowien (ID_Zamowienia, ID_Produktu, Ilosc, Cena) VALUES (@id_z, @id_p, @quan, @value)";
                _sqlConnection.Execute(sqlI, new { id_z = lastID.ID_Zamowienia + 1, id_p = productID, quan = quantity, value = finallPrice });

                ordersList.Add(opis);
                ListBoxRefresh(ordersList);
                progressID = lastID.ID_Zamowienia;
            }



        }

        private void btnCreateOrder(object sender, RoutedEventArgs e)
        {

            errorDiscount.Content = "";
            errorEmpty.Content = "";
            int res;
            if (!ordersList.Any())
            {
                errorEmpty.Content = "Nie mozna utworzyc pustego" + Environment.NewLine + "zamówienia!";
            }
            else if (!int.TryParse(txtDiscount.Text.Trim(), out res))
            {
                errorDiscount.Content += "Pole rabat musi być" + Environment.NewLine + "liczbą nieujamną!";


            }
            else if (String.IsNullOrEmpty(txtDiscount.Text))
            {
                errorDiscount.Content += "Pole rabat jest puste -" + Environment.NewLine + "rabat = 0!";

            }



            else if (int.TryParse(txtDiscount.Text.Trim(), out res) && (int.Parse(txtDiscount.Text.Trim()) < 0 || int.Parse(txtDiscount.Text.Trim()) > 100))
            {
                errorDiscount.Content += "Pole rabat nie może być" + Environment.NewLine + "ujemne!";
            }
            
            else
            {
                var discount = txtDiscount.Text;
                Klienci ID_klienta = comboClients.SelectedItem as Klienci;

                var sqlCreateOrder = "INSERT INTO Zamowienia (ID_Zamowienia, ID_Klienta, Rabat) VALUES (@id_z, @id_k, @disc)";
                _sqlConnection.Execute(sqlCreateOrder, new { id_z = progressID + 1, id_k = ID_klienta.ID_Klienta, disc = discount });
                ordersList.Clear();
                ListBoxRefresh(ordersList);
                ListBoxProductsRef();
            }

        }
    }
}
