using System.Collections.Generic;
using System.Threading.Tasks;
using CoronaDashboard.Models;
using RestEase;

namespace CoronaDashboard.Services
{
    public interface IStichtingNice
    {
        [Get("/intake-count")]
        Task<List<Entry>> GetIntakeCountAsync();
    }
}