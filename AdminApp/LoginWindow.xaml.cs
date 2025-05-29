using System;
using System.Text;
using System.Windows;
using System.Security.Cryptography;

namespace AdminApp
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameBox.Text.Trim();
            string password = PasswordBox.Password;

            // MD5 hash van "admin"
            string correctHash = "21232f297a57a5a743894a0e4a801fc3";

            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(password);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                if (username == "admin" && hash == correctHash)
                {
                    MainWindow mw = new MainWindow();
                    mw.Show();
                    this.Close();
                }
                else
                {
                    ErrorText.Text = "Foute gebruikersnaam of wachtwoord.";
                }
            }
        }
    }
}
