using CoronaDashboard.DataAccess.Options;

namespace CoronaDashboard
{
    public class CoronaDashboardOptions : CoronaDashboardDataAccessOptions
    {
        public int GroupByDays { get; set; }
    }
}