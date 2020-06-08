using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise.Charts;
using CoronaDashboard.Constants;
using CoronaDashboard.Localization;
using CoronaDashboard.Models;
using CoronaDashboard.Utils;

namespace CoronaDashboard.Services
{
    public class ChartService : IChartService
    {
        private readonly IDataService _dataService;

        public ChartService(IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<string> GetIntakeCountAsync(LineChart<double> chart)
        {
            var data = await _dataService.GetIntakeCountAsync();
            var grouped = GroupByDays(data);

            await chart.Clear();

            await chart.AddLabel(grouped.Select(d => DateUtils.ToShortDate(d.Date)).ToArray());

            var set = new LineChartDataset<double>
            {
                Fill = false,
                BorderColor = new List<string> { AppColors.ChartDarkBlue },
                Data = grouped.Select(d => d.Value).ToList()
            };
            await chart.AddDataSet(set);

            await chart.Update();

            return $"{DateUtils.ToLongDate(data.First().Date)} t/m {DateUtils.ToLongDate(data.Last().Date)}";
        }

        public async Task<string> GetDiedAndSurvivorsCumulativeAsync(LineChart<double> chart)
        {
            var data = await _dataService.GetDiedAndSurvivorsCumulativeAsync();
            var groupedOverleden = GroupByDays(data.Overleden);
            var groupedVerlaten = GroupByDays(data.Verlaten);
            var groupedNogOpVerpleegafdeling = GroupByDays(data.NogOpVerpleegafdeling);

            await chart.Clear();

            await chart.AddLabel(groupedOverleden.Select(d => DateUtils.ToShortDate(d.Date)).ToArray());

            var overleden = new LineChartDataset<double>
            {
                Fill = false,
                BorderColor = new List<string> { AppColors.ChartLightGray },
                Data = groupedOverleden.Select(d => d.Value).ToList()
            };
            await chart.AddDataSet(overleden);

            var verlaten = new LineChartDataset<double>
            {
                Fill = false,
                BorderColor = new List<string> { AppColors.Green },
                Data = groupedVerlaten.Select(d => d.Value).ToList()
            };
            await chart.AddDataSet(verlaten);

            var verpleegafdeling = new LineChartDataset<double>
            {
                Fill = false,
                BorderColor = new List<string> { AppColors.ChartBlue },
                Data = groupedNogOpVerpleegafdeling.Select(d => d.Value).ToList()
            };
            await chart.AddDataSet(verpleegafdeling);

            await chart.Update();

            return $"{DateUtils.ToLongDate(data.Overleden.First().Date)} t/m {DateUtils.ToLongDate(data.Overleden.Last().Date)}";
        }

        public async Task GetAgeDistributionStatusAsync(BarChart<int> chart)
        {
            var age = await _dataService.GetAgeDistributionStatusAsync();

            await chart.Clear();

            await chart.AddLabel(age.Leeftijdsverdeling.ToArray());

            var overleden = new BarChartDataset<int>
            {
                Label = Resources.Label_Overleden,
                BackgroundColor = age.Leeftijdsverdeling.Select(x => (string)AppColors.ChartLightGray),
                Data = age.Overleden
            };
            await chart.AddDataSet(overleden);

            var ic = new BarChartDataset<int>
            {
                Label = Resources.Label_IC,
                BackgroundColor = age.Leeftijdsverdeling.Select(x => (string)AppColors.ChartYellow),
                Data = age.NogOpgenomen
            };
            await chart.AddDataSet(ic);

            var verpleegafdeling = new BarChartDataset<int>
            {
                Label = Resources.Label_Verpleegafdeling,
                BackgroundColor = age.Leeftijdsverdeling.Select(x => (string)AppColors.ChartBlue),
                Data = age.ICVerlatenNogOpVerpleegafdeling
            };
            await chart.AddDataSet(verpleegafdeling);

            var gezond = new BarChartDataset<int>
            {
                Label = Resources.Label_Gezond,
                BackgroundColor = age.Leeftijdsverdeling.Select(x => (string)AppColors.ChartGreen),
                Data = age.ICVerlaten
            };
            await chart.AddDataSet(gezond);

            await chart.Update();
        }

        private static List<Entry<double>> GroupByDays(ICollection<Entry<int>> data, int days = 3)
        {
            long batchPeriod = TimeSpan.TicksPerDay * days;

            return data
                .GroupBy(entry => entry.Date.Ticks / batchPeriod)
                .Select(grouping => new Entry<double>
                {
                    Date = grouping.Select(e => e.Date).Max(),
                    Value = Math.Round(grouping.Select(e => e.Value).Average(), 1)
                })
                .ToList();
        }
    }
}
