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
        string AgeDistributionChartOptionsAsJson
        {
            get
            {
                var value = new
                {
                    animation = new { duration = 0 },
                    legend = new { display = false },
                    scales = new
                    {
                        xAxes = new[] { new { stacked = true, scaleLabel = new { display = true, labelString = Resources.AgeDistribution_X } } },
                        yAxes = new[] { new { stacked = true, scaleLabel = new { display = true, labelString = Resources.AgeDistribution_Y } } }
                    }
                };
                return JsonSerializer.Serialize(value);
            }
        }

        LineChart<double> DiedAndSurvivorsCumulative;
        LineChart<double> IntakeCount;
        BarChart<int> AgeDistribution;

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

                ChartService.GetAgeDistributionStatusAsync(AgeDistribution)
            };

            await Task.WhenAll(tasks);
        }
    }
}