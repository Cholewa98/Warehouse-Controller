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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Magazyn
{
 
    public partial class MainWindow : Window
    {
        private readonly IDbConnection _sqlConnection = SqlConnectionFactory.GetConnection();
        public void ListBoxRefresh(int sort)
        {
            listbox.Items.Clear();
            var sql = "SELECT ID_Towaru, Ilosc, Cena, Nazwa_Produktu FROM Towary T INNER JOIN Produkt P ON T.ID_Produktu = P.ID_Produktu";
            switch (sort)
            {
                case 0:
                sql += " ORDER BY Nazwa_Produktu";
                    break;
                case 1:
                    sql += " ORDER BY Nazwa_Produktu DESC";
                    break;
                case 2:
                    sql += " ORDER BY Ilosc";
                    break;
                case 3:
                    sql += " ORDER BY Ilosc DESC";
                    break;
                case 4:
                    sql += " ORDER BY Cena";
                    break;
                case 5:
                    sql += " ORDER BY Cena DESC";
                    break;
            }
            
            Result[] results = _sqlConnection.Query<Result>(sql).ToArray();

            //listbox.Items.Add(string.Format("{0,-30} {1,-20} {2,-20}", "Nazwa produktu:", "Ilosc:", "Cena:"));
            foreach (Result xd in results)
            {
                listbox.Items.Add(xd);
            }
            listbox.SelectedIndex = 0;
        }
        
        public MainWindow(string user)
        {
            string currentUser = user;

            

            InitializeComponent();

            comboSort.Items.Add("Nazwa produktu ->");
            comboSort.Items.Add("Nazwa produktu <-");
            comboSort.Items.Add("Ilość ->");
            comboSort.Items.Add("Ilość <-");
            comboSort.Items.Add("Cena ->");
            comboSort.Items.Add("Cena <-");
            comboSort.SelectedIndex = 0;

            labelUser.Content = "Zalogowany jako: "+currentUser;
            labelData.Content = string.Format("{0,-30} {1,15} {2,15}", "Nazwa produktu:", "Ilosc:", "Cena:");
            Result[] results = _sqlConnection.Query<Result>("SELECT ID_Towaru, Ilosc, Cena, Nazwa_Produktu FROM Towary T INNER JOIN Produkt P ON T.ID_Produktu = P.ID_Produktu").ToArray();

            //listbox.Items.Add(string.Format("{0,-30} {1,-20} {2,-20}", "Nazwa produktu:","Ilosc:", "Cena:"));
            //foreach (Result xd in results)
            //{
            //    listbox.Items.Add(xd);
            //}
            if (listbox.Items.Count != 0)
            {
                listbox.SelectedIndex = 0;
            }
            
            this.DataContext = this;
        }

        private void InfoBtn(object sender, RoutedEventArgs e)
        {
            if (listbox.SelectedItem != null)
            {
                Result selectedTowar = listbox.SelectedItem as Result;
                var selectedTowarID = selectedTowar.ID_Towaru;
                var sql = "SELECT ID_Towaru, Nazwa_Produktu, Nazwa_Producenta, Data, Ilosc, Cena FROM Produkt INNER JOIN Towary ON Produkt.ID_Produktu = Towary.ID_Produktu INNER JOIN Producenci ON Produkt.ID_Producenta = Producenci.ID_Producenta WHERE ID_Towaru = @SelectedTowarID";
                FullInfo fullInfo = _sqlConnection.Query<FullInfo>(sql, new { SelectedTowarID = selectedTowarID }).FirstOrDefault();
                MessageBox.Show(fullInfo.ToString());
            }
               
        }

        private void DeleteBtn(object sender, RoutedEventArgs e)
        {
            if (listbox.SelectedItem != null)
            {
                Result tes = listbox.SelectedItem as Result;
                var idToDelete = tes.ID_Towaru;
                MessageBox.Show("Usunięto wybraną partię");
                var sqlStatement = "DELETE FROM Towary WHERE ID_Towaru = @IdToDelete";
                var cotam = _sqlConnection.Execute(sqlStatement, new { IdToDelete = idToDelete });
                listbox.Items.Remove(listbox.SelectedItem);
            }
                
        }

        private void EditBtn(object sender, RoutedEventArgs e)
        {
            if (listbox.SelectedItem != null)
            {
                Result selectedItem = listbox.SelectedItem as Result;
                var selectedItemID = selectedItem.ID_Towaru;
                EditWindow editWindow = new EditWindow(selectedItemID);
                editWindow.Show();
                editWindow.Owner = this;
            }
            
            
        }

        private void sort(object sender, EventArgs e)
        {

            ComboBox comboBox = (ComboBox)sender;

            // Save the selected employee's name, because we will remove
            // the employee's name from the list.
            int selectedEmployee = comboSort.SelectedIndex;
            ListBoxRefresh(selectedEmployee);
        }

        private void CustomersBtn(object sender, RoutedEventArgs e)
        {
            CustomersWindow customersWindow = new CustomersWindow();
            customersWindow.Show();
        }

        private void OrdersBtn(object sender, RoutedEventArgs e)
        {
            OrdersWindow ordersWindow = new OrdersWindow();
            ordersWindow.Show();
        }

        private void RefreshBtn(object sender, RoutedEventArgs e)
        {
            ListBoxRefresh(comboSort.SelectedIndex);
            listbox.SelectedIndex = 0;
        }
    }
}
