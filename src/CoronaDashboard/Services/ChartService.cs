using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise.Charts;
using CoronaDashboard.Constants;
using CoronaDashboard.Localization;
using CoronaDashboard.Models;
using CoronaDashboard.Models.Api;
using CoronaDashboard.Utils;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace CoronaDashboard.Services
{
    public class ChartService : IChartService
    {
        private readonly IDataServiceFactory _factory;

        public ChartService(IDataServiceFactory factory)
        {
            _factory = factory;
        }

        public async Task<string> GetIntakeCount(string label, LineChart<double> chart)
        {
            var data = await _factory.GetClient().GetIntakeCountAsync();
            var grouped = GroupByDays(data);

            await chart.Clear();

            await chart.AddLabel(grouped.Select(d => DateUtils.ToShortDate(d.Date)).ToArray());

            var set = new LineChartDataset<double>
            {
                Fill = false,
                BorderColor = new List<string> { AppColors.LineChartBlue },
                PointRadius = 2,
                Data = grouped.Select(d => d.Value).ToList()
            };
            await chart.AddDataSet(set);

            await chart.Update();

            return $"{DateUtils.ToLongDate(data.First().Date)} t/m {DateUtils.ToLongDate(data.Last().Date)}";
        }

        public async Task GetAgeDistributionStatusAsync(BarChart<long> chart)
        {
            var data = await _factory.GetClient().GetAgeDistributionStatusAsync();

            var age = Map(data);

            await chart.Clear();

            await chart.AddLabel(age.Leeftijdsverdeling.ToArray());

            var overleden = new BarChartDataset<long>
            {
                Label = Resources.AgeDistribution_Label_Overleden,
                Data = age.Overleden
            };
            await chart.AddDataSet(overleden);

            var ic = new BarChartDataset<long>
            {
                Label = Resources.AgeDistribution_Label_IC,
                BackgroundColor = age.Leeftijdsverdeling.Select(x => (string)AppColors.BarChartYellow),
                Data = age.NogOpgenomen
            };
            await chart.AddDataSet(ic);

            var verpleegafdeling = new BarChartDataset<long>
            {
                Label = Resources.AgeDistribution_Label_Verpleegafdeling,
                BackgroundColor = age.Leeftijdsverdeling.Select(x => (string)AppColors.BarChartBlue),
                Data = age.ICVerlatenNogOpVerpleegafdeling
            };
            await chart.AddDataSet(verpleegafdeling);

            var gezond = new BarChartDataset<long>
            {
                Label = Resources.AgeDistribution_Label_Gezond,
                BackgroundColor = age.Leeftijdsverdeling.Select(x => (string)AppColors.BarChartGreen),
                Data = age.ICVerlaten
            };
            await chart.AddDataSet(gezond);

            await chart.Update();
        }

        private static AgeDistribution Map(object[][][] data)
        {
            return new AgeDistribution
            {
                Leeftijdsverdeling = data[0].Select(x => (string)x[0]).ToArray(),
                NogOpgenomen = data[0].Select(x => (long)x[1]).ToList(),
                ICVerlatenNogOpVerpleegafdeling = data[1].Select(x => (long)x[1]).ToList(),
                ICVerlaten = data[2].Select(x => (long)x[1]).ToList(),
                Overleden = data[3].Select(x => (long)x[1]).ToList()
            };
        }

        private static List<Entry> GroupByDays(ICollection<Entry> data, int days = 3)
        {
            long batchPeriod = TimeSpan.TicksPerDay * days;

            return data
                .GroupBy(entry => entry.Date.Ticks / batchPeriod)
                .Select(grouping => new Entry
                {
                    Date = grouping.Select(e => e.Date).Max(),
                    Value = Math.Round(grouping.Select(e => e.Value).Average(), 1)
                })
                .ToList();
        }
    }
}
