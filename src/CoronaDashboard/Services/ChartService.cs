using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise.Charts;
using CoronaDashboard.Constants;
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
                BorderColor = new List<string> { Color.Blue },
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

            var nogOpgenomenSet = new BarChartDataset<long>
            {
                //BorderColor = new List<string> { Color.Blue },
                //BackgroundColor = new List<string> { Color.Red },
                BorderWidth = 1,
                Data = age.NogOpgenomen
            };
            await chart.AddDataSet(nogOpgenomenSet);

            var ICVerlatenNogOpVerpleegafdelingSet = new BarChartDataset<long>
            {
                //BorderColor = new List<string> { Color.Blue },
                //BackgroundColor = new List<string> { Color.Red },
                BorderWidth = 1,
                Data = age.ICVerlatenNogOpVerpleegafdeling
            };
            await chart.AddDataSet(ICVerlatenNogOpVerpleegafdelingSet);

            var ICVerlatenSet = new BarChartDataset<long>
            {
                //BorderColor = new List<string> { Color.Blue },
                //BackgroundColor = new List<string> { Color.Red },
                BorderWidth = 1,
                Data = age.ICVerlaten
            };
            await chart.AddDataSet(ICVerlatenSet);

            List<string> backgroundColors = new List<string> { ChartColor.FromRgba(255, 99, 132, 0.2f), ChartColor.FromRgba(54, 162, 235, 0.2f), ChartColor.FromRgba(255, 206, 86, 0.2f), ChartColor.FromRgba(75, 192, 192, 0.2f), ChartColor.FromRgba(153, 102, 255, 0.2f), ChartColor.FromRgba(255, 159, 64, 0.2f) };
            List<string> borderColors = new List<string> { ChartColor.FromRgba(255, 99, 132, 1f), ChartColor.FromRgba(54, 162, 235, 1f), ChartColor.FromRgba(255, 206, 86, 1f), ChartColor.FromRgba(75, 192, 192, 1f), ChartColor.FromRgba(153, 102, 255, 1f), ChartColor.FromRgba(255, 159, 64, 1f) };

            var overledenSet = new BarChartDataset<long>
            {
                //BorderColor = borderColors,
                BackgroundColor = age.Leeftijdsverdeling.Select(x => (string) Color.Red),
                //BorderWidth = 1,
                Data = age.Overleden
            };
            await chart.AddDataSet(overledenSet);

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
