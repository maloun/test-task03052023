using demo.Models.Database.Tables;
using demo.Views.DbUpload;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Xml.Linq;
using demo.Models.Interfaces;

namespace demo.Models.Mocks
{
    public class DbUploadMock : Controller, IDbUpload
    {
        private readonly AppDbContext _dbContext;

        public DbUploadMock(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ObjectResult> UploadFile(UploadFilePostData data)
        {
            try
            {
                if (data.File == null)
                return BadRequest(new { error = "файл не может отсутствовать" });

                if (data.File.Length == 0)
                    return BadRequest(new { error = "размер файла должен быть больше 0" });
            
                if (data.File.Length > 1024 * 1024 * 5)
                    return BadRequest(new { error = "файл не должен превышать 5мб" });

                var timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                var fileName = $"wwwroot/upload/data_{timestamp}.xml";

                using (var stream = System.IO.File.Create(fileName))
                {
                    await data.File.CopyToAsync(stream);
                }
                var xml = XDocument.Load(fileName);
                UploadXmlToDatabase(xml);

                return Ok(new
                {
                    message = "файл успешно загружен",
                    received_bytes = data.File.Length
                });
            }
            catch (Exception e)
            {
                return BadRequest(new { error = $"ошибка распознавания файла {e.Message}" });
            }
        }

        public HouseConsumersTable ParseHouseConsumer(XElement element, DateTime _UploadDateTime)
        {
            var consumer = new HouseConsumersTable()
            {
                Name = element.Element("Name").Value,
                ConsumerId = Int32.Parse(element.Element("ConsumerId").Value),
                UploadDateTime = _UploadDateTime,
                Consumptions = new List<HouseConsumptionsTable>()
            };
                        
            foreach (var consumption in element.Elements("consumptions"))
            {
                consumer.Consumptions.Add(new HouseConsumptionsTable()
                {
                    Date = DateTime.Parse(consumption.Element("Date").Value),
                    Weather = float.Parse(consumption.Element("Weather").Value, CultureInfo.InvariantCulture),
                    Consumption = double.Parse(consumption.Element("Consumption").Value, CultureInfo.InvariantCulture)
                });
            }

            return consumer;
        }

        public PlantsConsumersTable ParsePlantsConsumer(XElement element, DateTime _UploadDateTime)
        {
            var consumer = new PlantsConsumersTable()
            {
                Name = element.Element("Name").Value,
                ConsumerId = Int32.Parse(element.Element("ConsumerId").Value),
                UploadDateTime = _UploadDateTime,
                Consumptions = new List<PlantsConsumptionsTable>()
            };

            foreach (var consumption in element.Elements("consumptions"))
            {
                consumer.Consumptions.Add(new PlantsConsumptionsTable()
                {
                    Date = DateTime.Parse(consumption.Element("Date").Value),
                    Price = double.Parse(consumption.Element("Price").Value, CultureInfo.InvariantCulture),
                    Consumption = double.Parse(consumption.Element("Consumption").Value, CultureInfo.InvariantCulture)
                });
            }

            return consumer;
        }

        public void UploadXmlToDatabase(XDocument xml)
        {
            var UploadTime = DateTime.Now;
            foreach (var element in xml.Root.Elements())
            {            
                switch (element.Name.ToString())
                {
                    case "houses":
                        _dbContext.HouseConsumers.Add(ParseHouseConsumer(element, UploadTime));
                        break;

                    case "plants":
                        _dbContext.PlantsConsumers.Add(ParsePlantsConsumer(element, UploadTime));
                        break;
                        
                    default:
                        throw new Exception($"неизвестный элемент {element.Name}");                            
                }            
            }

            _dbContext.SaveChanges();
        }
    }
}
