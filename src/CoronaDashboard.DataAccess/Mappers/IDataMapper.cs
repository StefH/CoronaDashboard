using System.Collections.Generic;
using System.Text.Json;
using CoronaDashboard.DataAccess.Models;
using CoronaDashboard.DataAccess.Models.Rijksoverheid;

namespace CoronaDashboard.DataAccess.Mappers;

public interface IDataMapper
{
    IReadOnlyCollection<TestedGGD> MapTestedGGD(TestedGGDDailyTotal data);

    BehandelduurDistribution MapBehandelduurDistribution(JsonElement[][][] data);

    AgeDistribution MapAgeDistribution(JsonElement[][][] data);

    DiedAndSurvivorsCumulative MapDiedAndSurvivorsCumulative(DateValueEntry<int>[][] data);
}