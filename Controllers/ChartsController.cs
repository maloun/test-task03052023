using Microsoft.AspNetCore.Mvc;
using demo.Models.Interfaces;
using demo.Views.Charts;
using Newtonsoft.Json;

namespace demo.Controllers
{
    public class ChartsController : Controller
    {
        ICharts _charts;
        public ChartsController(ICharts charts)
        {
            _charts = charts;
        }

        [HttpGet]
		public string GetChartsData(DateTime from, DateTime to)
		{
			return JsonConvert.SerializeObject(_charts.GetChartsData(from, to));			
		}
	
		[HttpGet]
		public ViewResult Index()
        {
            var obj = _charts.GetDefaultLimits();
			return View(obj);			
        }
    }
}
