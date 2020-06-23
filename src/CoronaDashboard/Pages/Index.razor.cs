using System.Collections.Generic;
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

        CardHeader IntakeCountHeader;
        ElementReference IntakeCountHeaderRef;
        ElementReference DiedAndSurvivorsCumulativeHeaderRef;
        ElementReference AgeDistributionHeaderRef;
        ElementReference BehandelduurDistributionHeaderRef;

        LineChart<double?> IntakeCountLineChart;
        LineChart<double> DiedAndSurvivorsCumulativeLineChart;
        BarChart<int> AgeDistributionBarChart;
        BarChart<int> BehandelduurDistributionBarChart;

        IntakeCountDetails IntakeCountDetails = new IntakeCountDetails { Dates = "..." };
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

            int header2Top = await JavaScriptInteropService.GetTop(DiedAndSurvivorsCumulativeHeaderRef);
            if (header2Top == header1Top)
            {
                await JavaScriptInteropService.SetClientHeight(DiedAndSurvivorsCumulativeHeaderRef, header1Height);
            }
            else
            {
                await JavaScriptInteropService.SetClientHeight(DiedAndSurvivorsCumulativeHeaderRef);
            }

            int header4Height = await JavaScriptInteropService.GetClientHeight(BehandelduurDistributionHeaderRef);
            int header4Top = await JavaScriptInteropService.GetTop(BehandelduurDistributionHeaderRef);

            int header3Top = await JavaScriptInteropService.GetTop(AgeDistributionHeaderRef);
            if (header3Top == header4Top)
            {
                await JavaScriptInteropService.SetClientHeight(AgeDistributionHeaderRef, header4Height);
            }
            else
            {
                await JavaScriptInteropService.SetClientHeight(AgeDistributionHeaderRef);
            }
        }

        async Task GetDataAsyncAndUpdateViewAsync()
        {
            var tasks = new[]
            {
                Task.Run(async () =>
                {
                    IntakeCountDetails = await ChartService.GetIntakeCountAsync(IntakeCountLineChart);
                    StateHasChanged();
                }),

                Task.Run(async () =>
                {
                    DiedAndSurvivorsCumulativeDates = await ChartService.GetDiedAndSurvivorsCumulativeAsync(DiedAndSurvivorsCumulativeLineChart);
                    StateHasChanged();
                }),

                ChartService.GetAgeDistributionStatusAsync(AgeDistributionBarChart),

                ChartService.GetBehandelduurDistributionAsync(BehandelduurDistributionBarChart)
            };

            await Task.WhenAll(tasks);
        }
    }
}