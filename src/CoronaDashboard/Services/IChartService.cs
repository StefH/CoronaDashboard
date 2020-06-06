using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blazorise.Charts;
using CoronaDashboard.Models;

namespace CoronaDashboard.Services
{
    public interface IChartService
    {
        Task<string> HandleRedraw(string label, LineChart<double> chart, Func<IStichtingNice, Task<List<Entry>>> func);
    }
}
