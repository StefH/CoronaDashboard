using System.Collections.Generic;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.Charts;
using CoronaDashboard.Constants;
using CoronaDashboard.Localization;
using CoronaDashboard.Models;
using CoronaDashboard.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;

namespace CoronaDashboard.Pages
{
    public partial class Index
    {
        private int GroupByDays = 5;

        [Inject]
        IConfiguration Configuration
        {
            set
            {
                GroupByDays = int.Parse(value["GroupByDays"]);
            }
        }

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

        DateRangeWithTodayValueDetails IntakeCountDetails = new DateRangeWithTodayValueDetails { Dates = AppConstants.D3, CountToday = AppConstants.D3 };
        DateRangeWithTodayValueDetails PositiefGetestePersonenPerDagDetails = new DateRangeWithTodayValueDetails { Dates = AppConstants.D3, CountToday = AppConstants.D3, CountTotal = AppConstants.D3 };
        DiedAndSurvivorsCumulativeDetails DiedAndSurvivorsCumulativeDetails = new DiedAndSurvivorsCumulativeDetails
        {
            Dates = AppConstants.D3,
            CountOverleden = AppConstants.D3,
            CountVerlaten = AppConstants.D3,
            CountNogOpVerpleegafdeling = AppConstants.D3,
        };

        // @string.Format(Resources.PositiefGetestePersonenPerDag_TodayAndTotal, PositiefGetestePersonenPerDagDetails.CountToday, PositiefGetestePersonenPerDagDetails.CountTotal)

        string PositiefGetestePersonenPerDagToday => string.Format(Resources.PositiefGetestePersonenPerDag_TodayAndTotal,
            PositiefGetestePersonenPerDagDetails.Today,
            PositiefGetestePersonenPerDagDetails.CountToday,
            PositiefGetestePersonenPerDagDetails.CountTotal);

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
                    PositiefGetestePersonenPerDagDetails = await ChartService.GetTestedGGDTotalAsync(PositiefGetestePersonenPerDagLineChart);
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