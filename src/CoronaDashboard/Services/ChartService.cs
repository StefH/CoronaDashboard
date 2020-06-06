using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise.Charts;
using CoronaDashboard.Constants;
using CoronaDashboard.Models;
using CoronaDashboard.Utils;

namespace CoronaDashboard.Services
{
    public class ChartService : IChartService
    {
        private readonly IDataServiceFactory _factory;

        public ChartService(IDataServiceFactory factory)
        {
            _factory = factory;
        }

        public async Task<string> HandleRedraw(string label, LineChart<double> chart, Func<IStichtingNice, Task<List<Entry>>> func)
        {
            var data = await func(_factory.GetClient());
            var grouped = GroupByDays(data);

            await chart.Clear();

            await chart.AddLabel(grouped.Select(d => DateUtils.ToShortDate(d.Date)).ToArray());

            var set = new LineChartDataset<double>
            {
                Fill = false,
                BorderColor = new List<string> { Color.Blue },
                PointRadius = 2,
                Data = grouped.Select(d => d.Value).ToList()
            };
            await chart.AddDataSet(set);

            await chart.Update();

            return $"{DateUtils.ToLongDate(data.First().Date)} t/m {DateUtils.ToLongDate(data.Last().Date)}";
        }

        private List<Entry> GroupByDays(ICollection<Entry> data, int days = 3)
        {
            long batchPeriod = TimeSpan.TicksPerDay * days;

            return data
                .GroupBy(entry => entry.Date.Ticks / batchPeriod)
                .Select(grouping => new Entry
                {
                    Date = grouping.Select(e => e.Date).Max(),
                    Value = Math.Round(grouping.Select(e => e.Value).Average(), 1)
                })
                .ToList();
        }
    }
}
