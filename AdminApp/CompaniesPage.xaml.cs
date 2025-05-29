using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BenchmarkLibrary.Data;
using BenchmarkLibrary.Models;

namespace AdminApp.Pages
{
    public partial class CompaniesPage : Page
    {
        private List<Company> bedrijven;

        public CompaniesPage()
        {
            InitializeComponent();
            this.Loaded += CompaniesPage_Loaded;
        }

        private void CompaniesPage_Loaded(object sender, RoutedEventArgs e)
        {
            LaadBedrijven();
        }

        private void LaadBedrijven()
        {
            bedrijven = Database.GetAllCompanies();
            string zoek = SearchBox.Text.Trim().ToLower();
            string statusFilter = ((ComboBoxItem)StatusFilterBox.SelectedItem).Content.ToString();

            var gefilterd = bedrijven
                .Where(b =>
                    (string.IsNullOrEmpty(zoek) || b.Name.ToLower().Contains(zoek)) &&
                    (statusFilter == "Alle" || b.Status.Equals(statusFilter, System.StringComparison.OrdinalIgnoreCase))
                ).ToList();

            CompanyListPanel.Children.Clear();

            foreach (var bedrijf in gefilterd)
            {
                var row = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 5, 0, 5) };

                if (bedrijf.Logo != null && bedrijf.Logo.Length > 0)
                {
                    var image = new Image
                    {
                        Width = 40,
                        Height = 40,
                        Margin = new Thickness(0, 0, 10, 0),
                        Source = ByteArrayToImageSource(bedrijf.Logo)
                    };
                    row.Children.Add(image);
                }

                var info = new TextBlock
                {
                    Text = $"Naam: {bedrijf.Name}    Status: {bedrijf.Status}",
                    Width = 400,
                    Foreground = bedrijf.Status switch
                    {
                        "active" => Brushes.Green,
                        "pending" => Brushes.Orange,
                        "rejected" => Brushes.Red,
                        _ => Brushes.Black
                    }
                };

                var btnEdit = new Button { Content = "✏", Width = 30, Margin = new Thickness(5, 0, 0, 0) };
                btnEdit.Click += (s, e) => BewerkenBedrijf(bedrijf);

                var btnDelete = new Button { Content = "🗑", Width = 30, Margin = new Thickness(5, 0, 0, 0) };
                btnDelete.Click += (s, e) => VerwijderBedrijf(bedrijf);

                var btnApprove = new Button { Content = "✔", Width = 30, Margin = new Thickness(5, 0, 0, 0) };
                btnApprove.Click += (s, e) => GoedkeurBedrijf(bedrijf);

                row.Children.Add(info);
                row.Children.Add(btnEdit);
                row.Children.Add(btnDelete);
                row.Children.Add(btnApprove);

                CompanyListPanel.Children.Add(row);
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            LaadBedrijven();
        }

        private void StatusFilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LaadBedrijven();
        }

        private void BewerkenBedrijf(Company bedrijf)
        {
            string nieuweNaam = Microsoft.VisualBasic.Interaction.InputBox("Nieuwe naam:", "Bedrijf bewerken", bedrijf.Name);
            if (!string.IsNullOrWhiteSpace(nieuweNaam) && nieuweNaam != bedrijf.Name)
            {
                bedrijf.Name = nieuweNaam;
                Database.UpdateCompany(bedrijf); // Appel à la class library
                LaadBedrijven();
            }
        }

        private void VerwijderBedrijf(Company bedrijf)
        {
            if (MessageBox.Show($"Weet je zeker dat je '{bedrijf.Name}' wilt verwijderen?", "Bevestiging", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Database.DeleteCompany(bedrijf.Id); // Appel à la class library
                LaadBedrijven();
            }
        }

        private void GoedkeurBedrijf(Company bedrijf)
        {
            Database.UpdateCompanyStatus(bedrijf.Id, "active"); // Appel à la class library
            LaadBedrijven();
        }

        private void BtnAddCompany_Click(object sender, RoutedEventArgs e)
        {
            var nieuwBedrijf = new Company
            {
                Name = "Nieuw bedrijf",
                Status = "pending"
            };
            Database.AddCompany(nieuwBedrijf); // Appel à la class library
            LaadBedrijven();
        }

        private ImageSource ByteArrayToImageSource(byte[] bytes)
        {
            using var ms = new System.IO.MemoryStream(bytes);
            var decoder = new PngBitmapDecoder(ms, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
            return decoder.Frames[0];
        }
    }
}
