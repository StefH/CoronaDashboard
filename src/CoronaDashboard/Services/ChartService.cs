using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise.Charts;
using CoronaDashboard.Constants;
using CoronaDashboard.Models;

namespace CoronaDashboard.Services
{
    public class ChartService : IChartService
    {
        private const string DateFormat = "dd-MM-yyyy";

        private readonly IDataServiceFactory _factory;

        public ChartService(IDataServiceFactory factory)
        {
            _factory = factory;
        }

        public async Task<string> HandleRedraw(string label, LineChart<int> chart, Func<IStichtingNice, Task<List<Entry>>> func)
        {
            var data = await func(_factory.GetClient());

            await chart.Clear();

            await chart.AddLabel(data.Select(d => d.Date.ToString(DateFormat)).ToArray());

            var set = new LineChartDataset<int>
            {
                //Label = label,
                Fill = false,
                BorderColor = new List<string> { Color.Blue },
                ShowLine = true,
                PointRadius = 0,
                Data = data.Select(d => d.Value).ToList()
            };
            await chart.AddDataSet(set);

            await chart.Update();

            return $"{data.First().Date.ToString(DateFormat)} t/m {data.Last().Date.ToString(DateFormat)}";
        }
    }
}
