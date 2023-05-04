using demo.Models.Database.Tables;
using demo.Views.DbUpload;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using demo.Models.Database.Tables;

namespace demo.Models.Interfaces
{
    public interface IDbUpload
    {
        public Task<ObjectResult> UploadFile(UploadFilePostData data);

        public HouseConsumersTable ParseHouseConsumer(XElement element);

        public PlantsConsumersTable ParsePlantsConsumer(XElement element);

        public void UploadXmlToDatabase(XDocument xml);
    }
}
