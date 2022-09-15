using System;
using CsvHelper.Configuration.Attributes;

namespace CoronaDashboard.DataAccess.Models.GitHubMZelst;

public class AllDataCsv
{
    [Name("date")]
    public DateTime Date { get; set; }

    [Name("positivetests")]
    [TypeConverter(typeof(NATypeConverter))]
    public int? PositiveTests { get; set; }

    [Name("values.tested_total")]
    [TypeConverter(typeof(NATypeConverter))]
    public int? TestedTotal { get; set; }
}