using AdminApp.Pages;
using System.Windows;
using System.Windows.Controls;

namespace AdminApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new AdminDashboardPage());
        }

        private void Dashboard_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new AdminDashboardPage());
        }

        private void Bedrijven_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CompaniesPage());
        }
    }
}
