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
        private readonly BlazoriseInteropServices _blazoriseInteropServices;

        public ChartService(IDataService dataService, BlazoriseInteropServices blazoriseInteropServices)
        {
            _dataService = dataService;
            _blazoriseInteropServices = blazoriseInteropServices;
        }

        public async Task<DateRangeWithTodayValueDetails> GetTestedGGDDailyTotalAsync(LineChart<double?> chart)
        {
            var allData = await _dataService.GetTestedGGDDailyTotalAsync();

            var groupedGeschat = GroupByDays(allData, tp => tp.Value);

            await chart.Clear();

            var set = new LineChartDataset<double?>
            {
                Fill = false,
                BorderColor = new List<string> { AppColors.ChartDarkBlue },
                Data = groupedGeschat.Select(d => (double?)d.Value).ToList()
            };

            double lastValue = allData.Last().Value;
            var points = Enumerable.Range(0, groupedGeschat.Count - 1).Select(x => (double?)null).ToList();
            points.Add(lastValue);

            var pointColors = Enumerable.Range(0, groupedGeschat.Count - 1).Select(x => (string)null).ToList();
            pointColors.Add(AppColors.ChartRed);
            var lastPoint = new LineChartDataset<double?>
            {
                Fill = false,
                PointBackgroundColor = pointColors,
                PointBorderColor = pointColors,
                Data = points
            };

            await _blazoriseInteropServices.AddLabelsDatasetsAndUpdate(chart.ElementId,
                GetLabelsWithYear(groupedGeschat.Select(g => g.Date)),
                set, lastPoint
            );

            return new DateRangeWithTodayValueDetails
            {
                Dates = $"{DateUtils.ToLongDate(allData.First().Date)} t/m {DateUtils.ToLongDate(allData.Last().Date)}",
                CountToday = lastValue.ToString(),
                CountTotal = allData.Sum(x => x.Value).ToString()
            };
        }

        public async Task<DateRangeWithTodayValueDetails> GetIntakeCountAsync(LineChart<double?> chart)
        {
            var data = await _dataService.GetIntakeCountAsync();
            var grouped = GroupByDays(data);

            await chart.Clear();

            var set = new LineChartDataset<double?>
            {
                Fill = false,
                BorderColor = new List<string> { AppColors.ChartDarkBlue },
                Data = grouped.Select(d => (double?)d.Value).ToList()
            };

            int lastValue = data.Last().Value;
            var points = Enumerable.Range(0, grouped.Count - 1).Select(x => (double?)null).ToList();
            points.Add(lastValue);

            var pointColors = Enumerable.Range(0, grouped.Count - 1).Select(x => (string)null).ToList();
            pointColors.Add(AppColors.ChartRed);
            var lastPoint = new LineChartDataset<double?>
            {
                Fill = false,
                PointBackgroundColor = pointColors,
                PointBorderColor = pointColors,
                Data = points
            };

            await _blazoriseInteropServices.AddLabelsDatasetsAndUpdate(chart.ElementId,
                GetLabelsWithYear(grouped.Select(g => g.Date)),
                set, lastPoint);

            return new DateRangeWithTodayValueDetails
            {
                Dates = $"{DateUtils.ToLongDate(data.First().Date)} t/m {DateUtils.ToLongDate(data.Last().Date)}",
                CountToday = lastValue.ToString()
            };
        }

        public async Task<DiedAndSurvivorsCumulativeDetails> GetDiedAndSurvivorsCumulativeAsync(LineChart<double> chart)
        {
            var data = await _dataService.GetDiedAndSurvivorsCumulativeAsync();
            var groupedOverleden = GroupByDays(data.Overleden);
            var groupedVerlaten = GroupByDays(data.Verlaten);
            var groupedNogOpVerpleegafdeling = GroupByDays(data.NogOpVerpleegafdeling);

            await chart.Clear();

            var overleden = new LineChartDataset<double>
            {
                Fill = false,
                BorderColor = new List<string> { AppColors.ChartLightGray },
                Data = groupedOverleden.Select(d => d.Value).ToList()
            };

            var verlaten = new LineChartDataset<double>
            {
                Fill = false,
                BorderColor = new List<string> { AppColors.Green },
                Data = groupedVerlaten.Select(d => d.Value).ToList()
            };

            var verpleegafdeling = new LineChartDataset<double>
            {
                Fill = false,
                BorderColor = new List<string> { AppColors.ChartBlue },
                Data = groupedNogOpVerpleegafdeling.Select(d => d.Value).ToList()
            };

            await _blazoriseInteropServices.AddLabelsDatasetsAndUpdate(chart.ElementId,
                GetLabelsWithYear(groupedOverleden.Select(g => g.Date)),
                overleden, verlaten, verpleegafdeling
            );

            return new DiedAndSurvivorsCumulativeDetails
            {
                Dates = $"{DateUtils.ToLongDate(data.Overleden.First().Date)} t/m {DateUtils.ToLongDate(data.Overleden.Last().Date)}",
                CountTodayOverleden = data.Overleden.Last().Value.ToString(),
                CountTodayNogOpVerpleegafdeling = data.NogOpVerpleegafdeling.Last().Value.ToString(),
                CountTodayVerlaten = data.Verlaten.Last().Value.ToString()
            };
        }

        public async Task GetAgeDistributionStatusAsync(BarChart<int> chart)
        {
            var age = await _dataService.GetAgeDistributionStatusAsync();

            await chart.Clear();

            var overleden = new BarChartDataset<int>
            {
                Label = Resources.Label_Overleden,
                BackgroundColor = age.LabelsLeeftijdsverdeling.Select(x => (string)AppColors.ChartLightGray),
                Data = age.Overleden
            };

            var ic = new BarChartDataset<int>
            {
                Label = Resources.Label_IC,
                BackgroundColor = age.LabelsLeeftijdsverdeling.Select(x => (string)AppColors.ChartYellow),
                Data = age.NogOpgenomen
            };

            var verpleegafdeling = new BarChartDataset<int>
            {
                Label = Resources.Label_Verpleegafdeling,
                BackgroundColor = age.LabelsLeeftijdsverdeling.Select(x => (string)AppColors.ChartBlue),
                Data = age.ICVerlatenNogOpVerpleegafdeling
            };

            var gezond = new BarChartDataset<int>
            {
                Label = Resources.Label_Gezond,
                BackgroundColor = age.LabelsLeeftijdsverdeling.Select(x => (string)AppColors.ChartGreen),
                Data = age.ICVerlaten
            };

            await chart.AddLabelsDatasetsAndUpdate(age.LabelsLeeftijdsverdeling.ToArray(), overleden, ic, verpleegafdeling, gezond);
        }

        public async Task GetBehandelduurDistributionAsync(BarChart<int> chart)
        {
            var age = await _dataService.GetBehandelduurDistributionAsync();

            await chart.Clear();

            var overleden = new BarChartDataset<int>
            {
                Label = Resources.Label_Overleden,
                BackgroundColor = age.LabelsDagen.Select(x => (string)AppColors.ChartLightGray),
                Data = age.Overleden
            };

            var ic = new BarChartDataset<int>
            {
                Label = Resources.Label_IC,
                BackgroundColor = age.LabelsDagen.Select(x => (string)AppColors.ChartYellow),
                Data = age.NogOpgenomen
            };

            var verpleegafdeling = new BarChartDataset<int>
            {
                Label = Resources.Label_Verpleegafdeling,
                BackgroundColor = age.LabelsDagen.Select(x => (string)AppColors.ChartBlue),
                Data = age.ICVerlatenNogOpVerpleegafdeling
            };

            var gezond = new BarChartDataset<int>
            {
                Label = Resources.Label_Gezond,
                BackgroundColor = age.LabelsDagen.Select(x => (string)AppColors.ChartGreen),
                Data = age.ICVerlaten
            };

            await chart.AddLabelsDatasetsAndUpdate(age.LabelsDagen.ToArray(), overleden, ic, verpleegafdeling, gezond);
        }

        private static List<DateValueEntry<double>> GroupByDays(IEnumerable<DateValueEntry<int>> data, int days = 3)
        {
            return GroupByDays(data, d => d.Value, days);
        }

        private static List<DateValueEntry<double>> GroupByDays<T>(IEnumerable<DateValueEntry<T>> data, Func<DateValueEntry<T>, double> selector, int days = 3)
        {
            long batchPeriod = TimeSpan.TicksPerDay * days;

            return data
                .GroupBy(entry => entry.Date.Ticks / batchPeriod)
                .Select(grouping => new DateValueEntry<double>
                {
                    Date = grouping.Select(e => e.Date).Max(),
                    Value = Math.Round(grouping.Select(selector).Average(), 1)
                })
                .ToList();
        }

        private static IEnumerable<object> GetLabelsWithYear(IEnumerable<DateTime> entries)
        {
            var years = new List<string>();
            foreach (var entry in entries.Select(e => new { Date = DateUtils.ToShortDate(e.Date), Year = e.Date.ToString("yyyy") }))
            {
                object label;
                if (!years.Contains(entry.Year))
                {
                    years.Add(entry.Year);
                    label = new[] { entry.Date, entry.Year };
                }
                else
                {
                    label = entry.Date;
                }

                yield return label;
            }
        }
    }
}