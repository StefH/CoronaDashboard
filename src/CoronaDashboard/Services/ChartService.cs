﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise.Charts;
using CoronaDashboard.Constants;
using CoronaDashboard.DataAccess.Models;
using CoronaDashboard.DataAccess.Services.Data;
using CoronaDashboard.Localization;
using CoronaDashboard.Utils;
using Microsoft.Extensions.Options;

namespace CoronaDashboard.Services
{
    public class ChartService : IChartService
    {
        private readonly int _groupByDays;
        private readonly IDataService _dataService;
        private readonly BlazoriseInteropServices _blazoriseInteropServices;

        public ChartService(IOptions<CoronaDashboardOptions> options, IDataService dataService, BlazoriseInteropServices blazoriseInteropServices)
        {
            _groupByDays = options.Value.GroupByDays;
            _dataService = dataService;
            _blazoriseInteropServices = blazoriseInteropServices;
        }

        public async Task<GGDDetails> GetTestedGGDAsync(LineChart<double?> chart)
        {
            var allData = await _dataService.GetTestedGGDAsync();
            var grouped = GroupByDays(allData, _groupByDays);

            await chart.Clear();

            var positive = new LineChartDataset<double?>
            {
                Fill = false,
                BorderColor = new List<string> { AppColors.ChartDarkBlue },
                Data = grouped.Select(d => d.Positive).ToList(),
                YAxisID = ChartConstants.GGDPositive,
                BorderWidth = 2,
                PointRadius = 2
            };

            var tested = new LineChartDataset<double?>
            {
                Fill = false,
                BorderColor = new List<string> { AppColors.ChartGray },
                Data = grouped.Select(d => d.Tested).ToList(),
                YAxisID = ChartConstants.GGDTested,
                BorderWidth = 2,
                PointRadius = 2
            };

            var positiveLastData = allData.Last();
            var testedLastData = allData.Last(x => x.Tested != null);
            int testedCount = grouped.Count(g => g.Date <= testedLastData.Date);
            int testedDifference = grouped.Count - testedCount;
            if (testedDifference == 1)
            {
                testedCount -= 1;
            }

            var positivePoints = Enumerable.Range(0, grouped.Count - 1).Select(_ => (double?)null).ToList();
            positivePoints.Add(positiveLastData.Positive);

            var positivePointColors = Enumerable.Range(0, grouped.Count - 1).Select(_ => (string)null).ToList();
            positivePointColors.Add(AppColors.ChartRed);

            var testedPoints = Enumerable.Range(0, testedCount).Select(_ => (double?)null).ToList();
            testedPoints.Add(testedLastData.Tested);

            var testedPointColors = Enumerable.Range(0, testedCount).Select(_ => (string)null).ToList();
            testedPointColors.Add(AppColors.ChartBlack);

            for (int i = 0; i < testedDifference; i++)
            {
                testedPoints.Add(null);
                testedPointColors.Add(null);
            }

            var positiveLastPoint = new LineChartDataset<double?>
            {
                Fill = false,
                PointBackgroundColor = positivePointColors,
                PointBorderColor = positivePointColors,
                Data = positivePoints,
                YAxisID = ChartConstants.GGDPositive,
                PointRadius = 2
            };

            var testedLastPoint = new LineChartDataset<double?>
            {
                Fill = false,
                PointBackgroundColor = testedPointColors,
                PointBorderColor = testedPointColors,
                Data = testedPoints,
                YAxisID = ChartConstants.GGDTested,
                PointRadius = 2
            };

            await chart.AddLabelsDatasetsAndUpdate(
                GetLabelsWithYear(grouped.Select(g => g.Date)).ToArray(),
                positive, tested, positiveLastPoint, testedLastPoint);

            return new GGDDetails
            {
                Dates = $"{DateUtils.ToDayWithShortMonthAndYear(allData.First().Date)} t/m {DateUtils.ToDayWithShortMonthAndYear(allData.Last().Date)}",

                Positive = $"{positiveLastData.Positive}",
                PositiveDate = DateUtils.ToTodayOrDayWithWithLongMonth(allData.Last().Date),
                PositiveTotal = $"{allData.Sum(x => x.Positive)}",

                Tested = $"{testedLastData.Tested}",
                TestedDate = DateUtils.ToTodayOrDayWithWithLongMonth(testedLastData.Date),
                TestedTotal = $"{allData.Sum(x => x.Tested ?? 0)}"
            };
        }

        public async Task<DateRangeWithTodayValueDetails> GetIntakeCountAsync(LineChart<double?> chart)
        {
            var data = await _dataService.GetIntakeCountAsync();
            var grouped = GroupByDays(data, _groupByDays);

            await chart.Clear();

            var set = new LineChartDataset<double?>
            {
                Fill = false,
                BorderColor = new List<string> { AppColors.ChartDarkBlue },
                Data = grouped.Select(d => (double?)d.Value).ToList(),
                BorderWidth = 2,
                PointRadius = 2
            };

            int lastValue = data.Last().Value;
            var points = Enumerable.Range(0, grouped.Count - 1).Select(_ => (double?)null).ToList();
            points.Add(lastValue);

            var pointColors = Enumerable.Range(0, grouped.Count - 1).Select(_ => (string)null).ToList();
            pointColors.Add(AppColors.ChartRed);
            var lastPoint = new LineChartDataset<double?>
            {
                Fill = false,
                PointBackgroundColor = pointColors,
                PointBorderColor = pointColors,
                Data = points,
                PointRadius = 2
            };

            await chart.AddLabelsDatasetsAndUpdate(
                GetLabelsWithYear(grouped.Select(g => g.Date)).ToArray(),
                set, lastPoint);

            return new DateRangeWithTodayValueDetails
            {
                Today = DateUtils.ToTodayOrDayWithWithLongMonth(data.Last().Date),
                Dates = $"{DateUtils.ToDayWithShortMonthAndYear(data.First().Date)} t/m {DateUtils.ToDayWithShortMonthAndYear(data.Last().Date)}",
                CountToday = lastValue.ToString()
            };
        }

        public async Task<DiedAndSurvivorsCumulativeDetails> GetDiedAndSurvivorsCumulativeAsync(LineChart<double> chart)
        {
            var data = await _dataService.GetDiedAndSurvivorsCumulativeAsync();
            var groupedOverleden = GroupByDays(data.Overleden, _groupByDays);
            var groupedVerlaten = GroupByDays(data.Verlaten, _groupByDays);
            var groupedNogOpVerpleegafdeling = GroupByDays(data.NogOpVerpleegafdeling, _groupByDays);

            await chart.Clear();

            var overleden = new LineChartDataset<double>
            {
                Fill = false,
                BorderColor = new List<string> { AppColors.ChartLightGray },
                Data = groupedOverleden.Select(d => d.Value).ToList(),
                BorderWidth = 2,
                PointRadius = 2
            };

            var verlaten = new LineChartDataset<double>
            {
                Fill = false,
                BorderColor = new List<string> { AppColors.Green },
                Data = groupedVerlaten.Select(d => d.Value).ToList(),
                BorderWidth = 2,
                PointRadius = 2
            };

            var verpleegafdeling = new LineChartDataset<double>
            {
                Fill = false,
                BorderColor = new List<string> { AppColors.ChartBlue },
                Data = groupedNogOpVerpleegafdeling.Select(d => d.Value).ToList(),
                BorderWidth = 2,
                PointRadius = 2
            };

            await chart.AddLabelsDatasetsAndUpdate(
                GetLabelsWithYear(groupedOverleden.Select(g => g.Date)).ToArray(),
                overleden, verlaten, verpleegafdeling);

            return new DiedAndSurvivorsCumulativeDetails
            {
                Dates = $"{DateUtils.ToDayWithShortMonthAndYear(data.Overleden.First().Date)} t/m {DateUtils.ToDayWithShortMonthAndYear(data.Overleden.Last().Date)}",
                CountOverleden = data.Overleden.Last().Value.ToString(),
                CountNogOpVerpleegafdeling = data.NogOpVerpleegafdeling.Last().Value.ToString(),
                CountVerlaten = data.Verlaten.Last().Value.ToString()
            };
        }

        public async Task GetAgeDistributionStatusAsync(BarChart<int> chart)
        {
            var age = await _dataService.GetAgeDistributionStatusAsync();

            await chart.Clear();

            var overleden = new BarChartDataset<int>
            {
                Label = Resources.Label_Overleden,
                BackgroundColor = age.LabelsLeeftijdsverdeling.Select(_ => (string)AppColors.ChartLightGray).ToArray(),
                Data = age.Overleden
            };

            var ic = new BarChartDataset<int>
            {
                Label = Resources.Label_IC,
                BackgroundColor = age.LabelsLeeftijdsverdeling.Select(_ => (string)AppColors.ChartYellow).ToArray(),
                Data = age.NogOpgenomen
            };

            var verpleegafdeling = new BarChartDataset<int>
            {
                Label = Resources.Label_Verpleegafdeling,
                BackgroundColor = age.LabelsLeeftijdsverdeling.Select(_ => (string)AppColors.ChartBlue).ToArray(),
                Data = age.ICVerlatenNogOpVerpleegafdeling
            };

            var gezond = new BarChartDataset<int>
            {
                Label = Resources.Label_Gezond,
                BackgroundColor = age.LabelsLeeftijdsverdeling.Select(_ => (string)AppColors.ChartGreen).ToArray(),
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
                BackgroundColor = age.LabelsDagen.Select(_ => (string)AppColors.ChartLightGray).ToArray(),
                Data = age.Overleden
            };

            var ic = new BarChartDataset<int>
            {
                Label = Resources.Label_IC,
                BackgroundColor = age.LabelsDagen.Select(_ => (string)AppColors.ChartYellow).ToArray(),
                Data = age.NogOpgenomen
            };

            var verpleegafdeling = new BarChartDataset<int>
            {
                Label = Resources.Label_Verpleegafdeling,
                BackgroundColor = age.LabelsDagen.Select(_ => (string)AppColors.ChartBlue).ToArray(),
                Data = age.ICVerlatenNogOpVerpleegafdeling
            };

            var gezond = new BarChartDataset<int>
            {
                Label = Resources.Label_Gezond,
                BackgroundColor = age.LabelsDagen.Select(_ => (string)AppColors.ChartGreen).ToArray(),
                Data = age.ICVerlaten
            };

            await chart.AddLabelsDatasetsAndUpdate(age.LabelsDagen.ToArray(), overleden, ic, verpleegafdeling, gezond);
        }

        private static List<DateValueEntry<double>> GroupByDays(IEnumerable<DateValueEntry<int>> data, int days)
        {
            return GroupByDays(data, d => d.Value, days);
        }

        private static List<DateValueEntry<double>> GroupByDays<T>(IEnumerable<DateValueEntry<T>> data, Func<DateValueEntry<T>, double> selector, int days)
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

        private static List<TestedGGD> GroupByDays(IEnumerable<TestedGGD> data, int days)
        {
            long batchPeriod = TimeSpan.TicksPerDay * days;

            TestedGGD Map(IGrouping<long, TestedGGD> grouping)
            {
                var totals = grouping.Where(t => t.Tested is not null).Select(t => t.Tested.Value).ToArray();
                double? averageTotal = totals.Any() ? Math.Round(totals.Average(), 1) : null;

                double? average = grouping.Select(t => t.Positive).Average();

                return new TestedGGD
                {
                    Date = grouping.Select(t => t.Date).Max(),
                    Positive = average is not null ? Math.Round(average.Value, 1) : null,
                    Tested = averageTotal
                };
            }

            return data
                .GroupBy(entry => entry.Date.Ticks / batchPeriod)
                .Select(Map)
                .ToList();
        }

        private static IEnumerable<object> GetLabelsWithYear(IEnumerable<DateTime> entries)
        {
            var years = new List<string>();
            foreach (var entry in entries.Select(e => new { Date = DateUtils.ToDayWithShortMonth(e.Date), Year = e.Date.ToString("yyyy") }))
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