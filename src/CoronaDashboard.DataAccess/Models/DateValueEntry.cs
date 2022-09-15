using System;

namespace CoronaDashboard.DataAccess.Models;

public class DateValueEntry<T>
{
    public DateTime Date { get; set; }

    public T Value { get; set; }
}