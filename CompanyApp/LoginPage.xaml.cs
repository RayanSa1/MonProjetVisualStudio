using System.Windows;
using BenchmarkLibrary.Data;
using BenchmarkLibrary.Models;

namespace CompanyApp
{
    public partial class LoginPage : Window
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginBox.Text.Trim();
            string password = PasswordBox.Password;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBlock.Text = "Gelieve alle velden in te vullen.";
                return;
            }

            var company = Database.CheckCompanyLogin(login, password);

            if (company != null)
            {
                MainWindow mw = new MainWindow(company);
                mw.Show();
                this.Close();
            }
            else
            {
                MessageBlock.Text = "Login mislukt of bedrijf niet actief.";
            }
        }
    }
}
