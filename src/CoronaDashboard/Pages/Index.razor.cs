﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.Charts;
using CoronaDashboard.Localization;
using CoronaDashboard.Models;
using CoronaDashboard.Options;
using CoronaDashboard.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;

namespace CoronaDashboard.Pages
{
    public partial class Index
    {
        private const string D3 = "...";

        [Inject]
        IOptions<ChartServiceOptions> Options { get; set; }

        [Inject]
        IChartService ChartService { get; set; }

        [Inject]
        JavaScriptInteropService JavaScriptInteropService { get; set; }

        readonly LineChartOptions PositiefGetestePersonenPerDagChartOptions = new LineChartOptions
        {
            // Animation = new Animation { Duration = 0 },
            Legend = new Legend { Display = false },
            Scales = new Scales
            {
                XAxes = new List<Axis> { new Axis { Display = true, ScaleLabel = new AxisScaleLabel { LabelString = Resources.PositiefGetestePersonenPerDag_X } } },
                YAxes = new List<Axis> { new Axis { Display = true, ScaleLabel = new AxisScaleLabel { LabelString = Resources.PositiefGetestePersonenPerDag_Y } } }
            }
        };

        readonly LineChartOptions IntakeCountChartOptions = new LineChartOptions
        {
            // Animation = new Animation { Duration = 0 },
            Legend = new Legend { Display = false },
            Scales = new Scales
            {
                XAxes = new List<Axis> { new Axis { Display = true, ScaleLabel = new AxisScaleLabel { LabelString = Resources.IntakeCount_X } } },
                YAxes = new List<Axis> { new Axis { Display = true, ScaleLabel = new AxisScaleLabel { LabelString = Resources.IntakeCount_Y } } }
            }
        };

        BarChartOptions AgeDistributionChartOptions => GetBarChartOptions(Resources.AgeDistribution_X, Resources.AgeDistribution_Y);

        BarChartOptions BehandelduurDistributionChartOptions => GetBarChartOptions(Resources.BehandelduurDistribution_X, Resources.BehandelduurDistribution_Y);

        BarChartOptions GetBarChartOptions(string x, string y)
        {
            return new BarChartOptions
            {
                // animation = new { duration = 0 },
                Legend = new Legend { Display = false },
                Scales = new Scales
                {
                    XAxes = new List<Axis> { new Axis { Stacked = true, ScaleLabel = new AxisScaleLabel { Display = true, LabelString = x } } },
                    YAxes = new List<Axis> { new Axis { Stacked = true, ScaleLabel = new AxisScaleLabel { Display = true, LabelString = y } } }
                }
            };
        }

        CardHeader IntakeCountHeader;
        ElementReference PositiefGetestePersonenPerDagHeaderRef;
        ElementReference IntakeCountHeaderRef;
        ElementReference DiedAndSurvivorsCumulativeHeaderRef;
        ElementReference AgeDistributionHeaderRef;
        ElementReference BehandelduurDistributionHeaderRef;

        LineChart<double?> PositiefGetestePersonenPerDagLineChart;
        LineChart<double?> IntakeCountLineChart;
        LineChart<double> DiedAndSurvivorsCumulativeLineChart;
        BarChart<int> AgeDistributionBarChart;
        BarChart<int> BehandelduurDistributionBarChart;

        DateRangeWithTodayValueDetails IntakeCountDetails = new DateRangeWithTodayValueDetails { Dates = D3, CountToday = D3 };
        DateRangeWithTodayValueDetails PositiefGetestePersonenPerDagDetails = new DateRangeWithTodayValueDetails { Dates = D3, CountToday = D3, CountTotal = D3 };
        DiedAndSurvivorsCumulativeDetails DiedAndSurvivorsCumulativeDetails = new DiedAndSurvivorsCumulativeDetails
        { 
            Dates = D3,
            CountTodayOverleden = D3,
            CountTodayVerlaten = D3,
            CountTodayNogOpVerpleegafdeling = D3,
        };

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

            int header2Top = await JavaScriptInteropService.GetTop(PositiefGetestePersonenPerDagHeaderRef);
            if (header2Top == header1Top)
            {
                await JavaScriptInteropService.SetClientHeight(PositiefGetestePersonenPerDagHeaderRef, header1Height);
            }
            else
            {
                await JavaScriptInteropService.SetClientHeight(PositiefGetestePersonenPerDagHeaderRef);
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
                    PositiefGetestePersonenPerDagDetails = await ChartService.GetTestedGGDDailyTotalAsync(PositiefGetestePersonenPerDagLineChart);
                    StateHasChanged();
                }),

                Task.Run(async () =>
                {
                    IntakeCountDetails = await ChartService.GetIntakeCountAsync(IntakeCountLineChart);
                    StateHasChanged();
                }),

                Task.Run(async () =>
                {
                    DiedAndSurvivorsCumulativeDetails = await ChartService.GetDiedAndSurvivorsCumulativeAsync(DiedAndSurvivorsCumulativeLineChart);
                    StateHasChanged();
                }),

                Task.Run(async () =>
                {
                    await ChartService.GetAgeDistributionStatusAsync(AgeDistributionBarChart);
                    StateHasChanged();
                }),

                Task.Run(async () =>
                {
                    await ChartService.GetBehandelduurDistributionAsync(BehandelduurDistributionBarChart);
                    StateHasChanged();
                })
            };

            await Task.WhenAll(tasks);
        }
    }
}