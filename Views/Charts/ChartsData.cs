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
		public IEnumerable<ChartPoint> Consumption;

		public IEnumerable<ChartPoint> LinearTrand;

		public IEnumerable<ChartPoint> Forecast;
	}
}