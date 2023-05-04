namespace demo.Models.Database.Tables
{
    public class HouseConsumersTable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int ConsumerId { get ; set; }

        public DateTime UploadDateTime { get; set; }

        public virtual ICollection<HouseConsumptionsTable> Consumptions { get; set;}
    }
}