using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise.Charts;
using CoronaDashboard.Constants;
using CoronaDashboard.Localization;
using CoronaDashboard.Models.Api;
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

        public async Task<string> GetIntakeCount(string label, LineChart<double> chart)
        {
            var data = await _dataService.GetIntakeCountAsync();
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

        public async Task GetAgeDistributionStatusAsync(BarChart<int> chart)
        {
            var age = await _dataService.GetAgeDistributionStatusAsync();

            await chart.Clear();

            await chart.AddLabel(age.Leeftijdsverdeling.ToArray());

            var overleden = new BarChartDataset<int>
            {
                Label = Resources.AgeDistribution_Label_Overleden,
                Data = age.Overleden
            };
            await chart.AddDataSet(overleden);

            var ic = new BarChartDataset<int>
            {
                Label = Resources.AgeDistribution_Label_IC,
                BackgroundColor = age.Leeftijdsverdeling.Select(x => (string)AppColors.BarChartYellow),
                Data = age.NogOpgenomen
            };
            await chart.AddDataSet(ic);

            var verpleegafdeling = new BarChartDataset<int>
            {
                Label = Resources.AgeDistribution_Label_Verpleegafdeling,
                BackgroundColor = age.Leeftijdsverdeling.Select(x => (string)AppColors.BarChartBlue),
                Data = age.ICVerlatenNogOpVerpleegafdeling
            };
            await chart.AddDataSet(verpleegafdeling);

            var gezond = new BarChartDataset<int>
            {
                Label = Resources.AgeDistribution_Label_Gezond,
                BackgroundColor = age.Leeftijdsverdeling.Select(x => (string)AppColors.BarChartGreen),
                Data = age.ICVerlaten
            };
            await chart.AddDataSet(gezond);

            await chart.Update();
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
