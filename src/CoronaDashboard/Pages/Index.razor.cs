using System.Collections.Generic;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.Charts;
using CoronaDashboard.Constants;
using CoronaDashboard.DataAccess.Models;
using CoronaDashboard.Localization;
using CoronaDashboard.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;

namespace CoronaDashboard.Pages
{
    public partial class Index
    {
        private int GroupByDays = 5;

        [Inject]
        IOptions<CoronaDashboardOptions> CoronaDashboardOptions
        {
            set => GroupByDays = value.Value.GroupByDays;
        }

        [Inject]
        IChartService ChartService { get; set; }

        [Inject]
        JavaScriptInteropService JavaScriptInteropService { get; set; }

        readonly LineChartOptions GGDGetestePersonenPerDagChartOptions = new()
        {
            // Animation = new Animation { Duration = 0 },
            Legend = new Legend { Display = false },
            Scales = new Scales
            {
                XAxes = new List<Axis> { new() { ScaleLabel = new AxisScaleLabel { LabelString = Resources.GGDGetestePersonenPerDag_X } } },
                YAxes = new List<Axis>
                {
                    new()
                    {
                        Id = ChartConstants.GGDPositive,
                        Position = "left",
                        Ticks = new AxisTicks
                        {
                            Callback = (value, index, values) => $"{value / 1000.0} K"
                        },
                        ScaleLabel = new AxisScaleLabel
                        {
                            Display = true,
                            Padding = 1,
                            FontColor = AppColors.ChartDarkBlue,
                            LabelString = $"{Resources.GGDPositiefGetestePersonenPerDag_Y}"
                        }
                    },
                    new()
                    {
                        Id = ChartConstants.GGDTested,
                        Position = "right",
                        GridLines = new AxisGridLines
                        {
                            Display = false
                        },
                        Ticks = new AxisTicks
                        {
                            Callback = (value, index, values) => $"{value / 1000.0} K"
                        },
                        ScaleLabel = new AxisScaleLabel
                        {
                            Display = true,
                            Padding = 1,
                            LabelString = $"{Resources.GGDGetestePersonenPerDag_Y}"
                        }
                    }
                }
            }
        };

        readonly object GGDGetestePersonenPerDagChartOptionsAsObject = new
        {
            Legend = new { Display = false },
            Scales = new
            {
                XAxes = new List<Axis> { new() { ScaleLabel = new AxisScaleLabel { LabelString = Resources.GGDGetestePersonenPerDag_X } } },
                YAxes = new List<Axis>
                {
                    new()
                    {
                        Id = ChartConstants.GGDPositive,
                        Position = "left",
                        Ticks = new AxisTicks
                        {
                            Callback = (value, index, values) => $"{value / 1000.0} K",
                            FontColor = AppColors.ChartDarkBlue
                        },
                        ScaleLabel = new AxisScaleLabel
                        {
                            Display = true,
                            Padding = 1,
                            FontColor = AppColors.ChartDarkBlue,
                            LabelString = $"{Resources.GGDPositiefGetestePersonenPerDag_Y}"
                        }
                    },
                    new()
                    {
                        Id = ChartConstants.GGDTested,
                        Position = "right",
                        GridLines = new AxisGridLines
                        {
                            Display = false
                        },
                        Ticks = new AxisTicks
                        {
                            Callback = (value, index, values) => $"{value / 1000.0} K",
                            FontColor = AppColors.ChartGray
                        },
                        ScaleLabel = new AxisScaleLabel
                        {
                            Display = true,
                            Padding = 1,
                            FontColor = AppColors.ChartGray,
                            LabelString = $"{Resources.GGDGetestePersonenPerDag_TodayAndTotal}"
                        }
                    }
                }
            }
        };

        readonly LineChartOptions IntakeCountChartOptions = new()
        {
            // Animation = new Animation { Duration = 0 },
            Legend = new Legend { Display = false },
            Scales = new Scales
            {
                XAxes = new List<Axis> { new() { ScaleLabel = new AxisScaleLabel { LabelString = Resources.IntakeCount_X } } },
                YAxes = new List<Axis> { new() { ScaleLabel = new AxisScaleLabel { LabelString = Resources.IntakeCount_Y } } }
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
                    XAxes = new List<Axis> { new() { Stacked = true, ScaleLabel = new AxisScaleLabel { Display = true, LabelString = x } } },
                    YAxes = new List<Axis> { new() { Stacked = true, ScaleLabel = new AxisScaleLabel { Display = true, LabelString = y } } }
                }
            };
        }

        CardHeader IntakeCountHeader;
        ElementReference GGDGetestePersonenPerDagHeaderRef;
        ElementReference IntakeCountHeaderRef;
        ElementReference DiedAndSurvivorsCumulativeHeaderRef;
        ElementReference AgeDistributionHeaderRef;
        ElementReference BehandelduurDistributionHeaderRef;

        LineChart<double?> GGDGetestePersonenPerDagLineChart;
        LineChart<double?> IntakeCountLineChart;
        LineChart<double> DiedAndSurvivorsCumulativeLineChart;
        BarChart<int> AgeDistributionBarChart;
        BarChart<int> BehandelduurDistributionBarChart;

        DateRangeWithTodayValueDetails IntakeCountDetails = new() { Dates = AppConstants.D3, CountToday = AppConstants.D3 };
        GGDDetails GGDDetails = new()
        {
            Dates = AppConstants.D3,

            PositiveDate = AppConstants.D3,
            Positive = AppConstants.D3,
            PositiveTotal = AppConstants.D3,

            TestedDate = AppConstants.D3,
            Tested = AppConstants.D3,
            TestedTotal = AppConstants.D3
        };
        DiedAndSurvivorsCumulativeDetails DiedAndSurvivorsCumulativeDetails = new()
        {
            Dates = AppConstants.D3,
            CountOverleden = AppConstants.D3,
            CountVerlaten = AppConstants.D3,
            CountNogOpVerpleegafdeling = AppConstants.D3
        };

        string PositiefGetestePersonenPerDagToday => string.Format(Resources.GGDPositiefGetestePersonenPerDag_TodayAndTotal,
            GGDDetails.PositiveDate,
            GGDDetails.Positive,
            GGDDetails.PositiveTotal);

        string GetestePersonenPerDagToday => string.Format(Resources.GGDGetestePersonenPerDag_TodayAndTotal,
            GGDDetails.TestedDate,
            GGDDetails.Tested,
            GGDDetails.TestedTotal);

        string IntakeCountToday => string.Format(Resources.IntakeCount_Today,
            IntakeCountDetails.Today,
            IntakeCountDetails.CountToday);

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

            int header2Top = await JavaScriptInteropService.GetTop(GGDGetestePersonenPerDagHeaderRef);
            if (header2Top == header1Top)
            {
                await JavaScriptInteropService.SetClientHeight(GGDGetestePersonenPerDagHeaderRef, header1Height);
            }
            else
            {
                await JavaScriptInteropService.SetClientHeight(GGDGetestePersonenPerDagHeaderRef);
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
                    GGDDetails = await ChartService.GetTestedGGDAsync(GGDGetestePersonenPerDagLineChart);
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