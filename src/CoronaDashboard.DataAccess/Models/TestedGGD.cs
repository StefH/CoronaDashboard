using System;

namespace CoronaDashboard.DataAccess.Models
{
    public class TestedGGD
    {
        public DateTime Date { get; set; }

        public double Positive { get; set; }

        public double? Tested { get; set; }
    }
}