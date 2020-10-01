using System;
using System.Text.Json.Serialization;

namespace CoronaDashboard.Models
{
    /// <summary>
    /// https://data.overheid.nl/dataset/12972-covid-19-besmettelijke-personen-per-dag
    /// </summary>
    public class BesmettelijkePersonenPerDag
    {
        /// <summary>
        /// Datum waarvoor het aantal besmettelijken is geschat
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Ondergrens 95% betrouwbaarheidsinterval
        /// </summary>
        [JsonPropertyName("prev_low")]
        public int Ondergrens { get; set; }

        /// <summary>
        /// Geschat aantal besmettelijken
        /// </summary>
        [JsonPropertyName("prev_avg")]
        public int Geschat { get; set; }

        /// <summary>
        /// 95% betrouwbaarheidsinterval
        /// </summary>
        [JsonPropertyName("prev_up")]
        public int Bovengrens { get; set; }

        /// <summary>
        /// patiëntpopulatie met waarde “hosp” voor gehospitaliseerde patiënten of “testpos” voor test-positieve patiënten
        /// </summary>
        public string Population { get; set; }
    }
}