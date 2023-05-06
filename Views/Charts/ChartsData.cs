namespace demo.Views.Charts
{
	public class TimePoint : ChartPoint<DateTime, double> { };
	public class ValuePoint : ChartPoint<double, double> { };

	public class ChartData
	{
		public IEnumerable<ValuePoint> LinearHouses;

		public IEnumerable<ValuePoint> LinearPlants;

		public TimePoint CityForecast;

		public CityConsumption CityConsumptions;

		public IEnumerable<ValuePoint> ConsumptionsPie;
	}

	public class NameValue
	{
		public string title;

		public double x;

		public double y;
	}

	public class CityConsumption
	{
		public IEnumerable<ValuePoint> Sum;

		public IEnumerable<NameValue> Houses;

		public IEnumerable<NameValue> Plants;

		public IEnumerable<NameValue> Pie;

	}

	public class ChartsLimits
	{
		public DateTime from;
		public DateTime to;
	}

	public class ChartPoint<X,Y>
	{
		public X x;
		public Y y;
	}
}