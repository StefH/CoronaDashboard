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

        private LineChartOptions Options = new LineChartOptions
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

        LineChart<int> IntakeCount;
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
            IntakeCountDates = await ChartService.HandleRedraw("Intake Count", IntakeCount, api => api.GetIntakeCountAsync());
            StateHasChanged();
        }
    }
}