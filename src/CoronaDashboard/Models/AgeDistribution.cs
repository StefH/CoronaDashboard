using System.Collections.Generic;

namespace CoronaDashboard.Models
{
    public class AgeDistribution
    {
        public string[] LabelsLeeftijdsverdeling { get; set; }

        /// <summary>
        /// Het aantal IC patiënten dat het ziekenhuis levend heeft verlaten.
        /// </summary>
        public List<int> ICVerlaten { get; set; }

        /// <summary>
        /// Het aantal patiënten dat de IC levend verlaten heeft maar nog in het ziekenhuis ligt.
        /// </summary>
        public List<int> ICVerlatenNogOpVerpleegafdeling { get; set; }

        /// <summary>
        /// Het aantal patiënten dat nog op de IC is opgenomen.
        /// </summary>
        public List<int> NogOpgenomen { get; set; }

        /// <summary>
        /// Het aantal patiënten dat bij IC opname is overleden.
        /// </summary>
        public List<int> Overleden { get; set; }
    }
}