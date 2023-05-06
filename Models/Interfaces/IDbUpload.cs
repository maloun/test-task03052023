using demo.Models.Database.Tables;
using demo.Views.DbUpload;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace demo.Models.Interfaces
{
    public interface IDbUpload
    {
        public Task<ObjectResult> UploadFile(UploadFilePostData data);

        public HouseConsumersTable ParseHouseConsumer(XElement element, DateTime _UploadDateTime);

        public PlantsConsumersTable ParsePlantsConsumer(XElement element, DateTime _UploadDateTime);

        public void UploadXmlToDatabase(XDocument xml);
    }
}
