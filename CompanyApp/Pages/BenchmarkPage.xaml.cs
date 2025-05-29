using System.Windows;
using System.Windows.Controls;
using BenchmarkLibrary.Models;
using BenchmarkLibrary.Data;
using LiveCharts.Wpf;
using LiveCharts;
using System.Collections.Generic;
using System.Linq;
using BenchmarkLibrary;

namespace CompanyApp.Pages
{
    public partial class BenchmarkPage : Page
    {
        private Company _company;

        public BenchmarkPage(Company company)
        {
            InitializeComponent();
            _company = company;
        }

        private void VergelijkButton_Click(object sender, RoutedEventArgs e)
        {
            string jaar = JaarBox.Text;

            // 1. Haal rapporten op
            List<Yearreport> rapporten = Database.GetYearReportsForSectorAndYear(_company.Nacecode, jaar);

            // 2. Zoek rapport van ingelogd bedrijf
            Yearreport mijnRapport = rapporten.FirstOrDefault(r => r.CompanyId == _company.Id);

            if (mijnRapport == null)
            {
                MessageBox.Show("Geen eigen rapport gevonden voor dat jaar.");
                return;
            }

            // 3. Vergelijk analyse uitvoeren
            var resultaten = BenchmarkAnalyse.Analyseer(mijnRapport, rapporten);

            // 4. Toon analyse
            AnalysePanel.Children.Clear();
            foreach (var r in resultaten)
            {
                TextBlock tb = new TextBlock
                {
                    Text = $"{r.Type.ToUpper()}: {r.Veld} – {r.Uitleg}",
                    Margin = new Thickness(5),
                    Foreground = r.Type == "Sterk punt" ? System.Windows.Media.Brushes.Green : System.Windows.Media.Brushes.Red
                };
                AnalysePanel.Children.Add(tb);
            }

            // 5. Grafiek opbouwen
            Chart.Series = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Mijn bedrijf",
                    Values = new ChartValues<int> { mijnRapport.Workplaces, mijnRapport.CleaningFrequency }
                },
                new ColumnSeries
                {
                    Title = "Gemiddelde",
                    Values = new ChartValues<int>
                    {
                        (int)rapporten.Average(r => r.Workplaces),
                        (int)rapporten.Average(r => r.CleaningFrequency)
                    }
                }
            };

            Chart.AxisX.Clear();
            Chart.AxisX.Add(new Axis
            {
                Labels = new[] { "Werkplekken", "Schoonmaak/week" }
            });

            Chart.AxisY.Clear();
            Chart.AxisY.Add(new Axis
            {
                Title = "Waarde"
            });
        }
    }
}
