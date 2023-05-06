using demo.Views.Charts;

namespace demo.Models.Interfaces
{
    public interface ICharts
	{
		public ChartData GetChartsData(DateTime from, DateTime to);	

		public ChartsLimits GetDefaultLimits();
	}
}