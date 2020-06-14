using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace CoronaDashboard.Services
{
    public class BlazoriseInteropServices
    {
        private readonly IJSRuntime _runtime;

        public BlazoriseInteropServices(IJSRuntime runtime)
        {
            _runtime = runtime;
        }

        public async Task<bool> AddChartLabels(string id, IEnumerable<object> labels)
        {
            return await _runtime.InvokeAsync<bool>("blazoriseCharts.addLabel", id, labels);
        }
    }
}