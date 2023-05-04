using Microsoft.AspNetCore.Mvc;
using demo.Models.Interfaces;

namespace demo.Controllers
{
    public class ChartsController : Controller
    {
        ICharts _charts;
        public ChartsController(ICharts charts)
        {
            _charts = charts;
        }

        public ViewResult Index()
        {
            return View(_charts.GetChartsData());
        }
    }
}
