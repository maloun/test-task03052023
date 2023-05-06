using demo.Models.Database.Tables;

namespace demo.Views.Charts
{
	public class ChartData
	{
		public IEnumerable<ChartPoint> LinearHouses;

		public IEnumerable<ChartPoint> LinearPlants;

		public ChartPoint CityForecast;

		public CityConsumption CityConsumptions;

		public IEnumerable<ChartPoint> ConsumptionsPie;
	}

	public class ChartPoint
	{
		public double x;

		public double y;

		public string title;

		public DateTime date;

		public static ChartPoint FromHouse(HouseConsumptionsTable house)
		{
			return new ChartPoint() {
				x = house.Weather,
				y = house.Consumption,
				title = house.Consumer.Name,
				date = house.Date
			};
		}
		public static ChartPoint FromPlants(PlantsConsumptionsTable plants)
		{
			return new ChartPoint() {
				x = plants.Price,
				y = plants.Consumption,
				title = plants.Consumer.Name,
				date = plants.Date
			};
		}
	}

	public class CityConsumption
	{
		public IEnumerable<ChartPoint> Sum;

		public IEnumerable<ChartPoint> Houses;

		public IEnumerable<ChartPoint> Plants;

		public IEnumerable<ChartPoint> Pie;
	}

	public class ChartsLimits
	{
		public DateTime from;
		public DateTime to;
	}
}