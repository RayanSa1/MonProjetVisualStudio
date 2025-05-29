using System.Windows;
using System.Windows.Controls;
using BenchmarkLibrary.Data;
using BenchmarkLibrary.Models;
using System.Collections.Generic;
using Microsoft.VisualBasic;

namespace CompanyApp.Pages
{
    public partial class ReportsPage : Page
    {
        private Company _company;

        public ReportsPage(Company company)
        {
            InitializeComponent();
            _company = company;
            LoadReports();
        }

        private void LoadReports()
        {
            ReportsPanel.Children.Clear();
            List<Yearreport> rapporten = Database.GetReportsForCompany(_company.Id);

            foreach (var report in rapporten)
            {
                StackPanel row = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(5) };

                row.Children.Add(new TextBlock { Text = $"Jaar: {report.Jaar}", Width = 100 });
                row.Children.Add(new TextBlock { Text = $"Werkplekken: {report.Workplaces}", Width = 150 });
                row.Children.Add(new TextBlock { Text = $"Schoonmaak/week: {report.CleaningFrequency}", Width = 180 });

                Button editBtn = new Button { Content = "✏️", Tag = report, Margin = new Thickness(5) };
                editBtn.Click += EditReport_Click;
                row.Children.Add(editBtn);

                Button deleteBtn = new Button { Content = "🗑️", Tag = report.Id, Margin = new Thickness(5) };
                deleteBtn.Click += DeleteReport_Click;
                row.Children.Add(deleteBtn);

                ReportsPanel.Children.Add(row);
            }
        }

        private void AddReport_Click(object sender, RoutedEventArgs e)
        {
            string jaar = NewYearBox.Text;
            if (!int.TryParse(NewWorkplacesBox.Text, out int workplaces)) return;
            if (!int.TryParse(NewCleaningBox.Text, out int cleaning)) return;

            Database.AddYearReport(_company.Id, jaar, workplaces, cleaning);
            LoadReports();
        }

        private void DeleteReport_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)((Button)sender).Tag;
            Database.DeleteYearReport(id);
            LoadReports();
        }

        private void EditReport_Click(object sender, RoutedEventArgs e)
        {
            Yearreport report = (Yearreport)((Button)sender).Tag;

            string nieuwWerkplekken = Interaction.InputBox("Nieuwe werkplekken:", "Wijzigen", report.Workplaces.ToString());
            string nieuwSchoonmaak = Interaction.InputBox("Nieuwe schoonmaakfrequentie:", "Wijzigen", report.CleaningFrequency.ToString());

            if (int.TryParse(nieuwWerkplekken, out int wp) && int.TryParse(nieuwSchoonmaak, out int cf))
            {
                Database.UpdateYearReport(report.Id, wp, cf);
                LoadReports();
            }
        }
    }
}
