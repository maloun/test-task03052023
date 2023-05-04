using demo.Models.Database.Tables;

namespace demo.Models.Database.Tables
{
    public class PlantsConsumersTable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int ConsumerId { get; set; }

        public DateTime UploadDateTime { get; set; }

        public virtual ICollection<PlantsConsumptionsTable> Consumptions { get; set; }
    }
}
