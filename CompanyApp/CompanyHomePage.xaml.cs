using BenchmarkLibrary.Models;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace CompanyApp.Pages
{
    public partial class CompanyHomePage : Page
    {
        private Company _company;

        public CompanyHomePage(Company company)
        {
            InitializeComponent();
            _company = company;
            ShowCompanyInfo();
        }

        private void ShowCompanyInfo()
        {
            WelcomeText.Text = $"Welkom, {_company.Name}!";

            if (_company.Logo != null)
            {
                BitmapImage bitmap = new BitmapImage();
                using (MemoryStream ms = new MemoryStream(_company.Logo))
                {
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = ms;
                    bitmap.EndInit();
                }
                LogoImage.Source = bitmap;
            }
        }
    }
}
