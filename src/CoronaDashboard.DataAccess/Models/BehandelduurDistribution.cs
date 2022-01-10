using System.Collections.Generic;

namespace CoronaDashboard.DataAccess.Models
{
    public class BehandelduurDistribution
    {
        public string[] LabelsDagen { get; set; }

        public List<int> ICVerlaten { get; set; }

        public List<int> ICVerlatenNogOpVerpleegafdeling { get; set; }

        public List<int> NogOpgenomen { get; set; }

        public List<int> Overleden { get; set; }
    }
}