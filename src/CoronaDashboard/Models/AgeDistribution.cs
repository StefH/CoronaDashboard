using System.Collections.Generic;

namespace CoronaDashboard.Models
{
    public class AgeDistribution
    {
        public string[] Leeftijdsverdeling { get; set; }

        /// <summary>
        /// Het aantal patiënten dat nog op de IC is opgenomen.
        /// </summary>
        public List<long> NogOpgenomen { get; set; }

        /// <summary>
        /// Het aantal patiënten dat de IC levend verlaten heeft maar nog in het ziekenhuis ligt.
        /// </summary>
        public List<long> ICVerlatenNogOpVerpleegafdeling { get; set; }

        /// <summary>
        /// Het aantal IC patiënten dat het ziekenhuis levend heeft verlaten.
        /// </summary>
        public List<long> ICVerlaten { get; set; }

        /// <summary>
        /// Het aantal patiënten dat bij IC opname is overleden.
        /// </summary>
        public List<long> Overleden { get; set; }
    }
}