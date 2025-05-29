using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkLibrary.Models;

namespace BenchmarkLibrary
{
    public static class BenchmarkAnalyse
    {
        public static List<BenchmarkResult> Analyseer(Yearreport mijnRapport, List<Yearreport> vergelijkgroep)
        {
            List<BenchmarkResult> resultaten = new();

            if (vergelijkgroep == null || vergelijkgroep.Count == 0)
                return resultaten;

            // 1. Moyenne du groupe
            double avgWorkplaces = vergelijkgroep.Average(r => r.Workplaces);
            double avgCleaning = vergelijkgroep.Average(r => r.CleaningFrequency);

            // 2. Comparaison - Workplaces
            if (mijnRapport.Workplaces > avgWorkplaces * 1.2)
            {
                resultaten.Add(new BenchmarkResult
                {
                    Type = "Sterk punt",
                    Veld = "Aantal werkplekken",
                    Uitleg = $"Je bedrijf heeft veel meer werkplekken dan gemiddeld ({mijnRapport.Workplaces} vs {avgWorkplaces:F1})."
                });
            }
            else if (mijnRapport.Workplaces < avgWorkplaces * 0.8)
            {
                resultaten.Add(new BenchmarkResult
                {
                    Type = "Zwak punt",
                    Veld = "Aantal werkplekken",
                    Uitleg = $"Je bedrijf heeft opvallend minder werkplekken dan gemiddeld ({mijnRapport.Workplaces} vs {avgWorkplaces:F1})."
                });
            }

            // 3. Comparaison - Cleaning frequency
            if (mijnRapport.CleaningFrequency > avgCleaning * 1.2)
            {
                resultaten.Add(new BenchmarkResult
                {
                    Type = "Sterk punt",
                    Veld = "Schoonmaakfrequentie",
                    Uitleg = $"Je schoonmaakfrequentie ligt duidelijk hoger dan het sectorgemiddelde ({mijnRapport.CleaningFrequency} vs {avgCleaning:F1})."
                });
            }
            else if (mijnRapport.CleaningFrequency < avgCleaning * 0.8)
            {
                resultaten.Add(new BenchmarkResult
                {
                    Type = "Zwak punt",
                    Veld = "Schoonmaakfrequentie",
                    Uitleg = $"Je bedrijf scoort lager op schoonmaakfrequentie dan gemiddeld ({mijnRapport.CleaningFrequency} vs {avgCleaning:F1})."
                });
            }

            return resultaten;
        }
    }
}
