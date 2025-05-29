using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkLibrary.Models
{
    public class Yearreport
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Jaar { get; set; }
        public int Workplaces { get; set; }
        public int CleaningFrequency { get; set; }
    }
}
