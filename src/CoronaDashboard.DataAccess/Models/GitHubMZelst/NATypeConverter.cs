using System;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace CoronaDashboard.DataAccess.Models.GitHubMZelst;

internal class NATypeConverter : ITypeConverter
{
    private const string NA = "NA";

    public object? ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        if (NA.Equals(text, StringComparison.InvariantCultureIgnoreCase) || !int.TryParse(text, out int value))
        {
            return null;
        }

        return value;
    }

    public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
    {
        throw new NotImplementedException();
    }
}