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

        public async Task<DateRangeWithTodayValueDetails> GetIntakeCountAsync(LineChart<double?> chart)
        {
            var data = await _dataService.GetIntakeCountAsync();
            var grouped = GroupByDays(data);

            await chart.Clear();

            await _blazoriseInteropServices.AddChartLabels(chart.ElementId, GetLabelsWithYear(grouped.Select(g => g.Date)));

            var set = new LineChartDataset<double?>
            {
                Fill = false,
                BorderColor = new List<string> { AppColors.ChartDarkBlue },
                Data = grouped.Select(d => (double?)d.Value).ToList()
            };
            await chart.AddDataSet(set);

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
            await chart.AddDataSet(lastPoint);

            await chart.Update();

            return new DateRangeWithTodayValueDetails
            {
                Dates = $"{DateUtils.ToLongDate(data.First().Date)} t/m {DateUtils.ToLongDate(data.Last().Date)}",
                Today = lastValue.ToString()
            };
        }

        public async Task<string> GetDiedAndSurvivorsCumulativeAsync(LineChart<double> chart)
        {
            var data = await _dataService.GetDiedAndSurvivorsCumulativeAsync();
            var groupedOverleden = GroupByDays(data.Overleden);
            var groupedVerlaten = GroupByDays(data.Verlaten);
            var groupedNogOpVerpleegafdeling = GroupByDays(data.NogOpVerpleegafdeling);

            await chart.Clear();

            await _blazoriseInteropServices.AddChartLabels(chart.ElementId, GetLabelsWithYear(groupedOverleden.Select(g => g.Date)));

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

        public async Task<DateRangeWithTodayValueDetails> GetBesmettelijkePersonenPerDagAsync(LineChart<double?> chart)
        {
            var allData = await _dataService.GetBesmettelijkePersonenPerDagAsync();
            var testedPositive = allData.Where(d =>
                (d.Value.Population == "hosp" && d.Date <= new DateTime(2020, 6, 12)) ||
                (d.Value.Population == "testpos" && d.Date > new DateTime(2020, 6, 12))
            );

            var groupedGeschat = GroupByDays(testedPositive, tp => tp.Value.Geschat);

            await chart.Clear();

            await _blazoriseInteropServices.AddChartLabels(chart.ElementId, GetLabelsWithYear(groupedGeschat.Select(g => g.Date)));

            var set = new LineChartDataset<double?>
            {
                Fill = false,
                BorderColor = new List<string> { AppColors.ChartDarkBlue },
                Data = groupedGeschat.Select(d => (double?)d.Value).ToList()
            };
            await chart.AddDataSet(set);

            int lastValue = allData.Last().Value.Geschat;
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
            await chart.AddDataSet(lastPoint);

            await chart.Update();

            return new DateRangeWithTodayValueDetails
            {
                Dates = $"{DateUtils.ToLongDate(testedPositive.First().Date)} t/m {DateUtils.ToLongDate(testedPositive.Last().Date)}",
                Today = lastValue.ToString()
            };
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