using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using testtask_tplus.Models;

namespace testtask_tplus.Controllers
{
    public class DbUploadController : Controller
    {
        private readonly ILogger<DbUploadController> _logger;

        public DbUploadController(ILogger<DbUploadController> logger)
        {
            _logger = logger;
        }

        public ViewResult Index()
        {
            return View();
        }
    }
}