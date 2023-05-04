namespace demo.Views.Charts
{
	public class ChartsLimits
	{
		public (DateTime from, DateTime to) HouseLimits;

		public (DateTime from, DateTime to) PlantsLimits;
	}

	public class ChartPoint
	{
		public DateTime x; 
		public double y;
	}

	public class ChartData
	{
		public IEnumerable<(DateTime x, double y)> Data;

		public IEnumerable<(DateTime x, double y)> Forecast;

		public ((DateTime x, double y) st, (DateTime x, double y) en) Linear;
	}
}
