using demo.Views.Charts;
using Microsoft.EntityFrameworkCore;
using demo.Models.Interfaces;
using System.Linq;

namespace demo.Models.Mocks
{
	public class ChartsMock : ICharts
	{
		private readonly AppDbContext _dbContext;

		public ChartsMock(AppDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public ChartsData GetChartsData()
		{
			return new ChartsData()
			{
				Houses = GetHousesData(),
				Plants = GetPlantsData()
			};
		}

		public AxisData GetHousesData()
		{
			/*
			var result = new AxisData();

			var consumptions = from d in _dbContext.HouseConsumers
				.Include(h => h.Consumptions)
				.OrderByDescending(c => c.UploadDateTime)
				.FirstOrDefault()
				.Consumptions
							   select new ChartPoint()
							   {
								   AxisX = d.Date,
								   AxisY = d.Weather
							   };
			double sum = 0;
			var trendData = consumptions.ToList();
			result.Period = trendData.Count;

			var window = result.Period * 0.1;

			for (int i = 0; i < trendData.Count; i++)
			{
				if (i <= window) {
					var point = new ChartPoint()
					{
						sum += trendData[i].AxisY;
						point.AxisZ = sum / (trendData.Count + 1);
					};
				}
			}
			*/
			throw new NotImplementedException();
		}
	


		public AxisData GetPlantsData()
		{/*
			var consumptions = from d in _dbContext.PlantsConsumers
				.Include(h => h.Consumptions)
				.OrderByDescending(c => c.UploadDateTime)
				.FirstOrDefault()
				.Consumptions
			select new ChartPoint()
			{
				AxisX = d.Date,
				AxisY = d.Price
			}; 
			
			return new AxisData()
			{
				Period = consumptions.Count,
				Data = consumptions
			};*/

			throw new NotImplementedException();
		}
	}
}
