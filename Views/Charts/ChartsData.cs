namespace demo.Views.Charts
{
	public class ChartPoint
	{
		public DateTime AxisX;

		public Double AxisY;

		public Double AxisZ;
	}

	public class AxisData
	{
		public IEnumerable<ChartPoint> Data;

		public int Period;
	}

	public class ChartsData
	{
		public AxisData Houses;

		public AxisData Plants;
	}
}
