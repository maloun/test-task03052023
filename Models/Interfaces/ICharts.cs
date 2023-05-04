using demo.Views.Charts;

namespace demo.Models.Interfaces
{
    public interface ICharts
	{
		public ChartData GetHousesData(DateTime from, DateTime to);

		public ChartData GetPlantsData(DateTime from, DateTime to);

		public (DateTime from, DateTime to) GetHouseLimits();

		public (DateTime from, DateTime to) GetPlantsLimits();
	}
}