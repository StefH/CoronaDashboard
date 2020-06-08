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
                await HandleRedraw();
            }
        }

        async Task HandleRedraw()
        {
            var t1 = Task.Run(async () =>
            {
                IntakeCountDates = await ChartService.GetIntakeCountAsync(IntakeCount);
                StateHasChanged();
            });

            var t2 = Task.Run(async () =>
            {
                DiedAndSurvivorsCumulativeDates = await ChartService.GetDiedAndSurvivorsCumulativeAsync(DiedAndSurvivorsCumulative);
                StateHasChanged();
            });

            var t3 = Task.Run(async () =>
            {
                await ChartService.GetAgeDistributionStatusAsync(AgeDistribution);
            });

            await Task.WhenAll(t1, t2, t3);

            //await Task.Run(async () =>
            //{
            //    IntakeCountDates = await ChartService.GetIntakeCountAsync(IntakeCount);
            //    StateHasChanged();
            //});

            //await Task.Run(async () =>
            //{
            //    DiedAndSurvivorsCumulativeDates = await ChartService.GetDiedAndSurvivorsCumulativeAsync(DiedAndSurvivorsCumulative);
            //    StateHasChanged();
            //});

            //await Task.Run(async () =>
            //{
            //    await ChartService.GetAgeDistributionStatusAsync(AgeDistribution);
            //});
        }
    }
}