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

	public class Linear
	{
		public ChartPoint st;
		public ChartPoint en;
	}

	public class ChartData
	{
		public IEnumerable<ChartPoint> Data;

		public IEnumerable<ChartPoint> Forecast;

		public Linear Linear;
	}
}
