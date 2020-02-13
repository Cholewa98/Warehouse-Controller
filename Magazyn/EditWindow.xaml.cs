using Dapper;
using Magazyn.Infrastructure;
using Magazyn.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    
    public partial class EditWindow : Window
    {
        private readonly IDbConnection _sqlConnection = SqlConnectionFactory.GetConnection();
        public int ID_Towaru { get; set; }
        public int ID_Produktu;
        public EditWindow(int id)
        {
            ID_Towaru = id;

            var sql = "SELECT * FROM Produkt";
            Produkt[] produkts = _sqlConnection.Query<Produkt>(sql).ToArray();
            var sqlProducenci = "SELECT * FROM Producenci";
            Producenci[] producencis = _sqlConnection.Query<Producenci>(sqlProducenci).ToArray();
            

            InitializeComponent();
            
            foreach (Producenci producenci in producencis)
            {
                comboProducer.Items.Add(producenci);
            }

            var sqlEdit = "SELECT Produkt.ID_Produktu, ID_Towaru, Nazwa_Produktu, Nazwa_Producenta, Data, Ilosc, Cena FROM Produkt INNER JOIN Towary ON Produkt.ID_Produktu = Towary.ID_Produktu INNER JOIN Producenci ON Produkt.ID_Producenta = Producenci.ID_Producenta WHERE ID_Towaru = @SelectedTowarID";
            string ID = ID_Towaru.ToString();
            
            FullInfo fullInfo = _sqlConnection.Query<FullInfo>(sqlEdit, new { SelectedTowarID = ID }).FirstOrDefault();
            txtPrice.Text = fullInfo.Cena.ToString();
            txtQuantity.Text = fullInfo.Ilosc.ToString();
            comboProducer.Text = fullInfo.Nazwa_Producenta;
      
            ID_Produktu = fullInfo.ID_Produktu;
        }

        public void submitBtn(object sender, RoutedEventArgs e)
        {
            errorPrice.Content = "";
            errorQuan.Content = "";
 
            var sql = "UPDATE Towary SET Cena = @Cena, Ilosc = @Ilosc WHERE ID_Towaru = @ID";
            float res;
            int res2;
            var id_producer=0;

            if (String.IsNullOrEmpty(txtPrice.Text))
            {
                errorPrice.Content += "Pole cena nie może być puste!";
                
            }
            
            else if (!float.TryParse(txtPrice.Text.Trim(), out res))
            {
                errorPrice.Content += "Pole cena musi być liczbą!";
                

            }

            else if (float.TryParse(txtPrice.Text.Trim(), out res) && float.Parse(txtPrice.Text.Trim()) <= 0)
            {
                errorPrice.Content += "Pole cena nie może być ujemne!";
            }



            else if (String.IsNullOrEmpty(txtQuantity.Text))
            {
                errorQuan.Content += "Pole cena nie może być puste!";

            }

            else if (!int.TryParse(txtQuantity.Text.Trim(), out res2))
            {
                errorQuan.Content += "Pole cena musi być liczbą!";


            }

            else if (int.TryParse(txtQuantity.Text.Trim(), out res2) && int.Parse(txtQuantity.Text.Trim()) <= 0)
            {
                errorQuan.Content += "Pole cena nie może być ujemne!";
            }


            else
            {
                Producenci producer = comboProducer.SelectedItem as Producenci;
                id_producer = producer.ID_Producenta;

                _sqlConnection.Execute(sql, new { Cena = float.Parse(txtPrice.Text), Ilosc = int.Parse(txtQuantity.Text), ID = ID_Towaru.ToString() });
                ((MainWindow)this.Owner).ListBoxRefresh(0);
                errorPrice.Content = "";
                errorQuan.Content = "";
                this.Close();
                var sqlP = "UPDATE Produkt SET ID_Producenta = @id_producenta WHERE ID_Produktu = @id_produktu";
                _sqlConnection.Query(sqlP, new { id_producenta = id_producer, id_produktu = ID_Produktu });
            }
            
            
        }
    }
}
