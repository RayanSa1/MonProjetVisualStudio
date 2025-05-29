using System.Windows;
using System.Windows.Controls;
using CompanyApp.Pages;
using BenchmarkLibrary.Models;


namespace CompanyApp
{
    public partial class MainWindow : Window
    {
        private Company
 currentCompany;

        public MainWindow(Company company)
        {
            InitializeComponent();
            currentCompany = company;

            // Charger la première page (optionnel)
            MainFrame.Navigate(new DashboardPage(currentCompany));
        }

        private void Dashboard_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new DashboardPage(currentCompany));
        }

        private void Benchmark_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new BenchmarkPage(currentCompany));
        }

        private void Reports_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ReportsPage(currentCompany));
        }
    }
}
