using Blazorise.Charts;

namespace CoronaDashboard.ChartJs
{
    public class MyLineChartDataset<T> : LineChartDataset<T>
    {
        public string YAxisID { get; set; }
    }
}