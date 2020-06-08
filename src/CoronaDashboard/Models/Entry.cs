using System;

namespace CoronaDashboard.Models
{
    public class Entry<T>
    {
        public DateTime Date { get; set; }

        public T Value { get; set; }
    }
}