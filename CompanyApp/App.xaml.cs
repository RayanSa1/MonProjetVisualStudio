using System.Windows;

namespace CompanyApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var login = new LoginPage();  // ou MainWindow si tu ne passes pas par login
            login.Show();
        }
    }
}
