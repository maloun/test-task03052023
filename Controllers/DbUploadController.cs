using demo.Views.DbUpload;
using Microsoft.AspNetCore.Mvc;
using demo.Models.Interfaces;

namespace demo.Controllers
{
    public class DbUploadController : Controller
    {
        private IDbUpload _dbUpload;

        public DbUploadController(IDbUpload dbUpload)
        {
            _dbUpload = dbUpload;
        }

        [HttpGet]
        public ViewResult UploadFile()
        {
            return View(new UploadFilePostData());
        }

        [HttpPost]
        public async Task<ObjectResult> UploadFile(UploadFilePostData data)
        {
            return await _dbUpload.UploadFile(data);
            //return Redirect("/Charts/Index");            
        }
    }
}