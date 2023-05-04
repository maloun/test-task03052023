namespace demo.Models.Database.Tables
{
    public class HouseConsumptionsTable
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public double Weather { get; set; }

        public double Consumption { get; set; }

        public int ConsumerId { get; set; }

        public virtual HouseConsumersTable Consumer { get; set; }
    }
}
