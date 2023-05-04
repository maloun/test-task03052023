using demo.Views.Charts;

namespace demo.Models.Interfaces
{
    public interface ICharts
	{
		public ChartsData GetChartsData();

		public AxisData GetHousesData();
		public AxisData GetPlantsData();
    }
}