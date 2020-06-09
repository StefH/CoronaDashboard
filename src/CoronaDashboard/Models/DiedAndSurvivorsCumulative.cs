using System.Collections.Generic;

namespace CoronaDashboard.Models
{
    public class DiedAndSurvivorsCumulative
    {
        /// <summary>
        /// index 0: Het aantal patiënten dat is overleden.
        /// </summary>
        public List<DateValueEntry<int>> Overleden { get; set; }

        /// <summary>
        /// index 1: Het aantal IC patiënten dat het ziekenhuis levend heeft verlaten.
        /// </summary>
        public List<DateValueEntry<int>> Verlaten { get; set; }

        /// <summary>
        /// index 2: Het aantal patiënten dat de IC levend verlaten heeft maar nog in het ziekenhuis ligt.
        /// </summary>
        public List<DateValueEntry<int>> NogOpVerpleegafdeling { get; set; }
    }
}