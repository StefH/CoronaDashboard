using System.Threading.Tasks;
using Blazorise.Charts;
using CoronaDashboard.Services;
using Microsoft.AspNetCore.Components;

namespace CoronaDashboard.Pages
{
    public partial class Index
    {
        [Inject]
        IChartService ChartService { get; set; }

        private LineChartOptions LineChartOptions = new LineChartOptions
        {
            Animation = new Animation { Duration = 0, Easing = "linear" },
            Legend = new Legend
            {
                Display = false
            },
            Tooltips = new Tooltips
            {
                Enabled = true
            }
        };

        private BarChartOptions AgeBarChartOptions = new BarChartOptions
        {
            Animation = new Animation { Duration = 0, Easing = "linear" },
            Legend = new Legend
            {
                Display = true
            },
            Tooltips = new Tooltips
            {
                Enabled = true
            }
        };

        LineChart<double> IntakeCount;
        BarChart<long> AgeDistribution;

        string IntakeCountDates;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await HandleRedraw();
            }
        }

        async Task HandleRedraw()
        {
            IntakeCountDates = await ChartService.GetIntakeCount("Intake Count", IntakeCount);

            await ChartService.GetAgeDistributionStatusAsync(AgeDistribution);

            //StateHasChanged();
        }
    }
}