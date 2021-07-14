using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Blazorise.Charts;
using Blazorise.Utilities;
using CoronaDashboard.ChartJs;
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

        public ValueTask AddLabelsDatasetsAndUpdate<T>(string id, IEnumerable<object> labels, params LineChartDataset<T>[] datasets)
        {
            //var sets = new List<object>();

            //foreach (var set in datasets)
            //{
            //    var dict = Converters.ToDictionary(set, true, false);
            //    Console.WriteLine("set  = " + JsonSerializer.Serialize(set));

            //    if (!string.IsNullOrEmpty(set.Label))
            //    {
            //        dict.Add("YAxisID", set.Label);
            //        Console.WriteLine("dict = " + JsonSerializer.Serialize(dict));

            //        sets.Add(dict);
            //    }
            //    else
            //    {
            //        sets.Add(set);
            //    }
            //}

            return _runtime.InvokeVoidAsync("blazoriseCharts.addLabelsDatasetsAndUpdate", id, labels, datasets);
        }

        public ValueTask AddLabelsDatasetsAndUpdate2<T>(string id, IEnumerable<object> labels, params ChartDataset<T>[] datasets)
        {
            var sets = new List<object>();

            foreach (var set in datasets)
            {
                if (set is MyLineChartDataset<T> myLineChartSet)
                {
                    var json = JsonSerializer.Serialize(myLineChartSet);
                    var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
                    dict.Add("yAxisID", myLineChartSet.YAxisID);
                    //sets.Add(dict);
                    //sets.Add(new
                    //{
                    //    Fill = myLineChartSet.Fill,
                    //    BorderColor = myLineChartSet.BorderColor,
                    //    BorderWidth = myLineChartSet.BorderWidth,
                    //    Data = myLineChartSet.Data,
                    //    YAxisID = myLineChartSet.YAxisID
                    //});

                    sets.Add(set);
                }
                else
                {
                    sets.Add(set);
                }
            }

            return _runtime.InvokeVoidAsync("blazoriseCharts.addLabelsDatasetsAndUpdate", id, labels, datasets);
        }
    }
}