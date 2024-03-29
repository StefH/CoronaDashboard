﻿using System.Collections.Generic;
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

        /// <summary>
        /// https://github.com/stsrki/Blazorise/issues/983
        /// Feature request: Add multiline labels support for ChartJS component 
        /// </summary>
        public ValueTask AddChartLabels2(string id, IEnumerable<object> labels)
        {
            return _runtime.InvokeVoidAsync("blazoriseCharts.addLabel", id, labels);
        }
    }
}