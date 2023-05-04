using demo.Models.Database.Tables;

namespace demo.Models.Database.Tables
{
    public class PlantsConsumptionsTable
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public double Price { get; set; }

        public double Consumption { get; set; }

        public int ConsumerId { get; set; }

        public virtual PlantsConsumersTable Consumer { get; set; }
    }
}
