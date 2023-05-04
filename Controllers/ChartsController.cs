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
		public string GetHouseData(DateTime from, DateTime to)
		{
			return JsonConvert.SerializeObject(_charts.GetHousesData(from, to));			
		}
	
		[HttpGet]
		public ActionResult GetPlantsData(DateTime from, DateTime to)
		{
			var jsonStr = JsonConvert.SerializeObject(_charts.GetPlantsData(from, to));

			return Json(jsonStr, "application/json");
		}

		[HttpGet]
		public ViewResult Index()
        {
			return View(new ChartsLimits() { 
                HouseLimits = _charts.GetHouseLimits(),   
                PlantsLimits = _charts.GetPlantsLimits()
            });			
        }
    }
}
