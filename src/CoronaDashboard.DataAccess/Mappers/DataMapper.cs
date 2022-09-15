using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using CoronaDashboard.DataAccess.Models;
using CoronaDashboard.DataAccess.Models.Rijksoverheid;

namespace CoronaDashboard.DataAccess.Mappers;

public class DataMapper : IDataMapper
{
    public IReadOnlyCollection<TestedGGD> MapTestedGGD(TestedGGDDailyTotal data)
    {
        return data.Values.Select(i => new TestedGGD
        {
            Date = DateTimeOffset.FromUnixTimeSeconds(i.DateUnix).DateTime,
            Positive = i.Infected,
            Tested = i.TestedTotal
        }).OrderBy(d => d.Date).ToList();
    }

    public BehandelduurDistribution MapBehandelduurDistribution(JsonElement[][][] data)
    {
        return new BehandelduurDistribution
        {
            LabelsDagen = data[0].Select(x => $"{x[0].GetInt32()}").ToArray(),
            ICVerlatenNogOpVerpleegafdeling = data[0].Select(x => x[1].GetInt32()).ToList(),
            NogOpgenomen = data[1].Select(x => x[1].GetInt32()).ToList(),
            ICVerlaten = data[2].Select(x => x[1].GetInt32()).ToList(),
            Overleden = data[3].Select(x => x[1].GetInt32()).ToList()
        };
    }

    public AgeDistribution MapAgeDistribution(JsonElement[][][] data)
    {
        return new AgeDistribution
        {
            LabelsLeeftijdsverdeling = data[0].Select(x => x[0].GetString()).ToArray(),
            NogOpgenomen = data[0].Select(x => x[1].GetInt32()).ToList(),
            ICVerlatenNogOpVerpleegafdeling = data[1].Select(x => x[1].GetInt32()).ToList(),
            ICVerlaten = data[2].Select(x => x[1].GetInt32()).ToList(),
            Overleden = data[3].Select(x => x[1].GetInt32()).ToList()
        };
    }

    public DiedAndSurvivorsCumulative MapDiedAndSurvivorsCumulative(DateValueEntry<int>[][] data)
    {
        return new DiedAndSurvivorsCumulative
        {
            Overleden = data[0].ToList(),
            Verlaten = data[1].ToList(),
            NogOpVerpleegafdeling = data[2].ToList(),
        };
    }
}