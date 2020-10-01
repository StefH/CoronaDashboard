﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.Charts;
using CoronaDashboard.Localization;
using CoronaDashboard.Models;
using CoronaDashboard.Services;
using Microsoft.AspNetCore.Components;

namespace CoronaDashboard.Pages
{
    public partial class Index
    {
        [Inject]
        IChartService ChartService { get; set; }

        [Inject]
        JavaScriptInteropService JavaScriptInteropService { get; set; }

        readonly object BesmettelijkePersonenPerDagChartOptionsObject = new
        {
            // Animation = new Animation { Duration = 0 },
            legend = new { display = false },
            scales = new
            {
                xAxes = new[] { new { scaleLabel = new { display = true, labelString = Resources.BesmettelijkePersonenPerDag_X } } },
                yAxes = new[] { new { scaleLabel = new { display = true, labelString = Resources.BesmettelijkePersonenPerDag_Y } } }
            }
        };

        readonly object IntakeCountChartOptionsObject = new
        {
            // Animation = new Animation { Duration = 0 },
            legend = new { display = false },
            scales = new
            {
                xAxes = new[] { new { scaleLabel = new { display = true, labelString = Resources.IntakeCount_X } } },
                yAxes = new[] { new { scaleLabel = new { display = true, labelString = Resources.IntakeCount_Y } } }
            }
        };

        readonly LineChartOptions IntakeCountChartOptions = new LineChartOptions
        {
            // Animation = new Animation { Duration = 0 },
            Legend = new Legend { Display = false },
            Scales = new Scales
            {
                XAxes = new List<Axis> { new Axis { Display = true, ScaleLabel = new AxeScaleLabel { LabelString = Resources.IntakeCount_X } } },
                YAxes = new List<Axis> { new Axis { Display = true, ScaleLabel = new AxeScaleLabel { LabelString = Resources.IntakeCount_Y } } }
            }
        };

        object AgeDistributionChartOptionsObject => GetBarChartOptionObject(Resources.AgeDistribution_X, Resources.AgeDistribution_Y);

        object BehandelduurDistributionChartOptionsObject => GetBarChartOptionObject(Resources.BehandelduurDistribution_X, Resources.BehandelduurDistribution_Y);

        BarChartOptions AgeDistributionChartOptions => GetBarChartOptions(Resources.AgeDistribution_X, Resources.AgeDistribution_Y);

        BarChartOptions BehandelduurDistributionChartOptions => GetBarChartOptions(Resources.BehandelduurDistribution_X, Resources.BehandelduurDistribution_Y);

        object GetBarChartOptionObject(string x, string y)
        {
            return new
            {
                // animation = new { duration = 0 },
                legend = new { display = false },
                scales = new
                {
                    xAxes = new[] { new { stacked = true, scaleLabel = new { display = true, labelString = x } } },
                    yAxes = new[] { new { stacked = true, scaleLabel = new { display = true, labelString = y } } }
                }
            };
        }

        BarChartOptions GetBarChartOptions(string x, string y)
        {
            return new BarChartOptions
            {
                // animation = new { duration = 0 },
                Scales = new Scales
                {
                    XAxes = new List<Axis> { new Axis { Display = true, ScaleLabel = new AxeScaleLabel { LabelString = x } } },
                    YAxes = new List<Axis> { new Axis { Display = true, ScaleLabel = new AxeScaleLabel { LabelString = y } } }
                }
            };
        }

        CardHeader IntakeCountHeader;
        ElementReference BesmettelijkePersonenPerDagHeaderRef;
        ElementReference IntakeCountHeaderRef;
        ElementReference DiedAndSurvivorsCumulativeHeaderRef;
        ElementReference AgeDistributionHeaderRef;
        ElementReference BehandelduurDistributionHeaderRef;

        LineChart<double?> BesmettelijkePersonenPerDagLineChart;
        LineChart<double?> IntakeCountLineChart;
        LineChart<double> DiedAndSurvivorsCumulativeLineChart;
        BarChart<int> AgeDistributionBarChart;
        BarChart<int> BehandelduurDistributionBarChart;

        DateRangeWithTodayValueDetails IntakeCountDates = new DateRangeWithTodayValueDetails { Dates = "...", Today = "..." };
        DateRangeWithTodayValueDetails BesmettelijkePersonenPerDagDates = new DateRangeWithTodayValueDetails { Dates = "...", Today = "..." };
        string DiedAndSurvivorsCumulativeDates = "...";

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JavaScriptInteropService.SetEventListener();
                JavaScriptInteropService.OnResizeOrRotate += async delegate { await FixHeaders(); };

                await GetDataAsyncAndUpdateViewAsync();
                await FixHeaders();
            }
        }

        async Task FixHeaders()
        {
            int header1Height = await JavaScriptInteropService.GetClientHeight(IntakeCountHeaderRef);
            int header1Top = await JavaScriptInteropService.GetTop(IntakeCountHeaderRef);

            int header2Top = await JavaScriptInteropService.GetTop(BesmettelijkePersonenPerDagHeaderRef);
            if (header2Top == header1Top)
            {
                await JavaScriptInteropService.SetClientHeight(BesmettelijkePersonenPerDagHeaderRef, header1Height);
            }
            else
            {
                await JavaScriptInteropService.SetClientHeight(BesmettelijkePersonenPerDagHeaderRef);
            }

            int header4Height = await JavaScriptInteropService.GetClientHeight(AgeDistributionHeaderRef);
            int header4Top = await JavaScriptInteropService.GetTop(AgeDistributionHeaderRef);

            int header3Top = await JavaScriptInteropService.GetTop(DiedAndSurvivorsCumulativeHeaderRef);
            if (header3Top == header4Top)
            {
                await JavaScriptInteropService.SetClientHeight(DiedAndSurvivorsCumulativeHeaderRef, header4Height);
            }
            else
            {
                await JavaScriptInteropService.SetClientHeight(DiedAndSurvivorsCumulativeHeaderRef);
            }

            //int header1Height = await JavaScriptInteropService.GetClientHeight(IntakeCountHeaderRef);
            //int header1Top = await JavaScriptInteropService.GetTop(IntakeCountHeaderRef);

            //int header2Top = await JavaScriptInteropService.GetTop(DiedAndSurvivorsCumulativeHeaderRef);
            //if (header2Top == header1Top)
            //{
            //    await JavaScriptInteropService.SetClientHeight(DiedAndSurvivorsCumulativeHeaderRef, header1Height);
            //}
            //else
            //{
            //    await JavaScriptInteropService.SetClientHeight(DiedAndSurvivorsCumulativeHeaderRef);
            //}

            //int header4Height = await JavaScriptInteropService.GetClientHeight(BehandelduurDistributionHeaderRef);
            //int header4Top = await JavaScriptInteropService.GetTop(BehandelduurDistributionHeaderRef);

            //int header3Top = await JavaScriptInteropService.GetTop(AgeDistributionHeaderRef);
            //if (header3Top == header4Top)
            //{
            //    await JavaScriptInteropService.SetClientHeight(AgeDistributionHeaderRef, header4Height);
            //}
            //else
            //{
            //    await JavaScriptInteropService.SetClientHeight(AgeDistributionHeaderRef);
            //}
        }

        async Task GetDataAsyncAndUpdateViewAsync()
        {
            var tasks = new[]
            {
                Task.Run(async () =>
                {
                    IntakeCountDates = await ChartService.GetIntakeCountAsync(IntakeCountLineChart);
                    StateHasChanged();
                }),

                Task.Run(async () =>
                {
                    DiedAndSurvivorsCumulativeDates = await ChartService.GetDiedAndSurvivorsCumulativeAsync(DiedAndSurvivorsCumulativeLineChart);
                    StateHasChanged();
                }),

                ChartService.GetAgeDistributionStatusAsync(AgeDistributionBarChart),

                ChartService.GetBehandelduurDistributionAsync(BehandelduurDistributionBarChart),

                Task.Run(async () =>
                {
                    BesmettelijkePersonenPerDagDates = await ChartService.GetBesmettelijkePersonenPerDagAsync(BesmettelijkePersonenPerDagLineChart);
                    StateHasChanged();
                }),
            };

            await Task.WhenAll(tasks);
        }
    }
}