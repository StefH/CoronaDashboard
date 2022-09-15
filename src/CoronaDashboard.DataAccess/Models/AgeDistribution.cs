using System.Collections.Generic;

namespace CoronaDashboard.DataAccess.Models;

public class AgeDistribution
{
    public string[] LabelsLeeftijdsverdeling { get; set; }

    public List<int> ICVerlaten { get; set; }

    public List<int> ICVerlatenNogOpVerpleegafdeling { get; set; }

    public List<int> NogOpgenomen { get; set; }

    public List<int> Overleden { get; set; }
}