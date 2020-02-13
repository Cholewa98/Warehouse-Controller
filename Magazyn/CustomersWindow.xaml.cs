using Dapper;
using Magazyn.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// <summary>
    /// Logika interakcji dla klasy CustomersWindow.xaml
    /// </summary>
    public partial class CustomersWindow : Window
    {
        private readonly IDbConnection _sqlConnection = SqlConnectionFactory.GetConnection();

        public void RefCustomerList()
        {
            listboxCustomers.Items.Clear();
            var sql = "SELECT * FROM Klienci";
            Klienci[] klienci = _sqlConnection.Query<Klienci>(sql).ToArray();

            foreach (Klienci klient in klienci)
            {
                listboxCustomers.Items.Add(klient);
            }
        }

        public static bool IsPhoneNumber(string number)
        {
            return Regex.Match(number, @"^(\+[0-9]{11})$").Success;
        }

        public CustomersWindow()
        {
            InitializeComponent();
            labelCustomers.Content = string.Format("{0,-30} {1,-20} {2,-30}", "Nazwa klienta:", "Numer telefonu:", "Adres:");

            //SqlConnection sqlConnection = new SqlConnection(@"Data Source=DESKTOP-1EFN2CA\SQLEXPRESS;" + "Initial Catalog=Magazyn;" + "Integrated Security=SSPI;");
            //var sql = "SELECT * FROM Klienci";
            //Klienci[] klienci = sqlConnection.Query<Klienci>(sql).ToArray();

            //foreach (Klienci klient in klienci)
            //{
            //    listboxCustomers.Items.Add(klient);
            //}
            RefCustomerList();
        }

        private void addCustomerBtn(object sender, RoutedEventArgs e)
        {
            var customerName = txtCustomerName;
            var customerAdress = txtCustomerAdress;
            var customerNumber = txtPhone;

            errorPhone.Content = "";
            errorCustomerName.Content = "";
            errorAdres.Content = "";

            bool ifCustomer = false;
            var sql1 = "SELECT * FROM Klienci";
            Klienci[] klienci = _sqlConnection.Query<Klienci>(sql1).ToArray();
            foreach (Klienci klient in klienci)
            {
                if (klient.Nazwa_Klienta == txtCustomerName.Text)
                {
                    ifCustomer = true;
                }
            }
            if (String.IsNullOrEmpty(txtCustomerName.Text))
            {
                errorCustomerName.Content += "Pole ilosc nie może być" + Environment.NewLine +"puste!";

            }
            else if (ifCustomer == true)
            {
                errorCustomerName.Content += "Taki klient już istnieje!";
            }
            else if (txtCustomerName.Text.Length > 40 || !Regex.IsMatch(txtCustomerName.Text, @"[a-zA-Z\s]"))
            {
                errorCustomerName.Content += "Błędna nazwa!";
            }
            else if (String.IsNullOrEmpty(txtCustomerAdress.Text))
            {
                errorAdres.Content += "Pole adres nie może być" + Environment.NewLine +"puste!";

            }
            else if (txtCustomerAdress.Text.Length > 40 || !Regex.IsMatch(txtCustomerAdress.Text, @"[a-zA-Z0-9\s]"))
            {
                errorAdres.Content += "Błędny adres!";
            }
            else if (String.IsNullOrEmpty(txtPhone.Text))
            {
                errorPhone.Content += "Pole telefon nie może być" + Environment.NewLine +"puste!";

            }
            else if (!IsPhoneNumber(txtPhone.Text))
            {
                errorPhone.Content += "Należy podać numer" + Environment.NewLine +"telefonu!";
                //MessageBox.Show("to nie telefon!");
            }

            else
            {
                var sql = "INSERT INTO Klienci (Nazwa_Klienta, Numer_Kontaktowy, Adres) VALUES (@name, @number, @adress) ";
                SqlConnection sqlConnection = new SqlConnection(@"Data Source=DESKTOP-1EFN2CA\SQLEXPRESS;" + "Initial Catalog=Magazyn;" + "Integrated Security=SSPI;");
                sqlConnection.Execute(sql, new { name = customerName.Text, number = customerNumber.Text, adress = customerAdress.Text });
                RefCustomerList();
            }
            
        }
    }
}
