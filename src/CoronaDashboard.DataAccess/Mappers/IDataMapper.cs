﻿using System.Collections.Generic;
using System.Text.Json;
using CoronaDashboard.Models;
using CoronaDashboard.Models.Rijksoverheid;

namespace CoronaDashboard.DataAccess.Mappers
{
    public interface IDataMapper
    {
        IReadOnlyCollection<DateValueEntry<double>> MapTestedGGDDailyTotal(TestedGGDDailyTotal data);

        BehandelduurDistribution MapBehandelduurDistribution(JsonElement[][][] data);

        AgeDistribution MapAgeDistribution(JsonElement[][][] data);

        DiedAndSurvivorsCumulative MapDiedAndSurvivorsCumulative(DateValueEntry<int>[][] data);
    }
}