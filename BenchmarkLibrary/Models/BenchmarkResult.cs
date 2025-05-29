namespace BenchmarkLibrary.Models
{
    public class BenchmarkResult
    {
        public string Type { get; set; }         // "Sterk punt" of "Zwak punt"
        public string Veld { get; set; }         // Bijvoorbeeld "Aantal werkplekken"
        public string Uitleg { get; set; }       // Gedetailleerde uitleg
    }
}
