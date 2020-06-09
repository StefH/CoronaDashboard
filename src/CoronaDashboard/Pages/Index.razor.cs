using System.Text.Json;
using System.Threading.Tasks;
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

        LineChart<double> DiedAndSurvivorsCumulative;
        LineChart<double> IntakeCount;
        BarChart<int> AgeDistribution;
        BarChart<int> BehandelduurDistribution;

        string IntakeCountDates = "...";
        string DiedAndSurvivorsCumulativeDates = "...";

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await GetDataAsyncAndUpdateViewAsync();
            }
        }

        async Task GetDataAsyncAndUpdateViewAsync()
        {
            var tasks = new[]
            {
                Task.Run(async () =>
                {
                    IntakeCountDates = await ChartService.GetIntakeCountAsync(IntakeCount);
                    StateHasChanged();
                }),

                Task.Run(async () =>
                {
                    DiedAndSurvivorsCumulativeDates =
                        await ChartService.GetDiedAndSurvivorsCumulativeAsync(DiedAndSurvivorsCumulative);
                    StateHasChanged();
                }),

                ChartService.GetAgeDistributionStatusAsync(AgeDistribution),

                ChartService.GetBehandelduurDistributionAsync(BehandelduurDistribution)
            };

            await Task.WhenAll(tasks);
        }
    }
}