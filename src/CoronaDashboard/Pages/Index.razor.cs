﻿using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.Charts;
using CoronaDashboard.Localization;
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

        string IntakeCountChartOptionsAsJson
        {
            get
            {
                var value = new
                {
                    animation = new { duration = 0 },
                    legend = new { display = false },
                    scales = new
                    {
                        xAxes = new[] { new { scaleLabel = new { display = true, labelString = Resources.IntakeCount_X } } },
                        yAxes = new[] { new { scaleLabel = new { display = true, labelString = Resources.IntakeCount_Y } } }
                    }
                };
                return JsonSerializer.Serialize(value);
            }
        }
        string AgeDistributionChartOptionsAsJson =>
            GetBarChartOptionsAsJson(Resources.AgeDistribution_X, Resources.AgeDistribution_Y);

        private string BehandelduurDistributionChartOptionsAsJson =>
            GetBarChartOptionsAsJson(Resources.BehandelduurDistribution_X, Resources.BehandelduurDistribution_Y);

        string GetBarChartOptionsAsJson(string x, string y)
        {
            var value = new
            {
                animation = new { duration = 0 },
                legend = new { display = false },
                scales = new
                {
                    xAxes = new[] { new { stacked = true, scaleLabel = new { display = true, labelString = x } } },
                    yAxes = new[] { new { stacked = true, scaleLabel = new { display = true, labelString = y } } }
                }
            };
            return JsonSerializer.Serialize(value);
        }

        CardHeader IntakeCountHeader;
        ElementReference CardDeck1;
        ElementReference IntakeCountHeaderRef;
        ElementReference DiedAndSurvivorsCumulativeHeaderRef;

        LineChart<double> IntakeCountLineChart;
        LineChart<double> DiedAndSurvivorsCumulativeLineChart;
        BarChart<int> AgeDistributionBarChart;
        BarChart<int> BehandelduurDistributionBarChart;

        string IntakeCountDates = "...";
        string DiedAndSurvivorsCumulativeDates = "...";

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JavaScriptInteropService.SetEventListener();
                JavaScriptInteropService.OnResize += async delegate { await FixHeaders(); };

                await GetDataAsyncAndUpdateViewAsync();
                await FixHeaders();
            }
        }

        async Task FixHeaders()
        {
            int header1Height = await JavaScriptInteropService.GetClientHeight(IntakeCountHeaderRef);
            int header1TopTop = await JavaScriptInteropService.GetTop(IntakeCountHeaderRef);

            int header2Top = await JavaScriptInteropService.GetTop(DiedAndSurvivorsCumulativeHeaderRef);
            if (header2Top == header1TopTop)
            {
                await JavaScriptInteropService.SetClientHeight(DiedAndSurvivorsCumulativeHeaderRef, header1Height);
            }
            else
            {
                await JavaScriptInteropService.SetClientHeight(DiedAndSurvivorsCumulativeHeaderRef);
            }
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

                ChartService.GetBehandelduurDistributionAsync(BehandelduurDistributionBarChart)
            };

            await Task.WhenAll(tasks);
        }
    }
}