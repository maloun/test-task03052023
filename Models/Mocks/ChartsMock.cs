using demo.Views.Charts;
using Microsoft.EntityFrameworkCore;
using demo.Models.Interfaces;

namespace demo.Models.Mocks
{
	public static class LinqExtensions
	{
		public static IEnumerable<double> AverageWindow<T>(this IEnumerable<T> source, int size,
			Func<T, double> selector) 
		{
			var buffer = new Queue<T>(size);
			foreach (var item in source)
			{
				buffer.Enqueue(item);
				if (buffer.Count == size)
				{
					yield return buffer.ToList().Select(selector).Average();
					buffer.Dequeue();
				}
			}
		}
	}

	public class ChartsMock : ICharts
	{
		private readonly AppDbContext _dbContext;

		public ChartsMock(AppDbContext dbContext)
		{
			_dbContext = dbContext;
		}
				
		public static (double slope, double intercept) CalcLinearRegressionCoefficients<T>(
			ICollection<T> data, Func<T, double> xSelector, Func<T, double> ySelector)
		{
			double sumX = 0, sumY = 0, sumXY = 0, sumX2 = 0;
			int n = data.Count;

			foreach (T item in data)
			{
				double x = xSelector(item);
				double y = ySelector(item);

				sumX += x;
				sumY += y;
				sumXY += x * y;
				sumX2 += x * x;
			}

			double slope = (n * sumXY - sumX * sumY) / (n * sumX2 - sumX * sumX);
			double intercept = (sumY - slope * sumX) / n;

			return (slope, intercept);
		}

		public ChartData GetChartData(ICollection<ChartPoint> data, DateTime from, DateTime to) 
		{
			// сглаживание - скользящее окно, размер окна 15
			var window = 15;
			var smooth = data
				.AverageWindow(window, d => d.y)
				.Zip(
					data.Select(d => d.x.AddDays(window)),
					(a, b) => new ChartPoint() { x = b, y = a }
				).ToList();

			// сглаживание - экспоненциальное 
			double alpha = 0.3;
			smooth = smooth.Select((value, index) =>
			{
				if (index == 0)
					return value;
				else
					return new ChartPoint()
					{
						x = value.x,
						y = alpha * value.y + (1 - alpha) * smooth.ElementAt(index - 1).y,
					};
			}).ToList();
						
			// сезонность  
			var values = smooth.Select(d => d.y).ToList();
			var average = values.Average();
			var half = values.Count / 2;
			var seasonality = smooth.Select(d => d.y)
				  .Select((value, index) => new { value, index })
				  .Where((value, index) => index < half)
				  .GroupBy(item => values[item.index + half], item => item.value)
				  .Select(group => group.Sum() / average / 2)
				  .ToArray();
			
			var coeffs = CalcLinearRegressionCoefficients(smooth, d => d.x.Ticks, d => d.y);

			return new ChartData()
			{
				Consumption = data,
				
				LinearTrand = new List<ChartPoint>() {
					new ChartPoint() {
						x = data.Min(dp => dp.x),
						y = coeffs.intercept + coeffs.slope * data.Min(dp => dp.x).Ticks
					},
					new ChartPoint() {
						x = data.Max(dp => dp.x),
						y = coeffs.intercept + coeffs.slope * data.Max(dp => dp.x).Ticks
					}
				},

				Forecast = smooth
					.Where((value, index) => index < seasonality.Length)
					.Select((value, index) => new ChartPoint()
					{
						x = value.x.AddDays(smooth.Count),
						y = (coeffs.intercept + coeffs.slope * value.x.Ticks) * seasonality[index]
					})
			};
		}
		
		public ChartData GetHousesData(DateTime from, DateTime to) 
		{
			var data = _dbContext.HouseConsumers
				.Include(p => p.Consumptions)
				.OrderByDescending(c => c.UploadDateTime)
				.FirstOrDefault()
				.Consumptions
				.Where(d => d.Date > from && d.Date < to)
				.Select(d => new ChartPoint() { x = d.Date, y = d.Weather })
				.ToList();

			return GetChartData(data, from, to);
		}

		public ChartData GetPlantsData(DateTime from, DateTime to)
		{
			var data = _dbContext.PlantsConsumers
				.Include(p => p.Consumptions)
				.OrderByDescending(c => c.UploadDateTime)
				.FirstOrDefault()
				.Consumptions
				.Where(d => d.Date > from && d.Date < to)
				.Select(d => new ChartPoint() { x = d.Date, y = d.Price })
				.ToList();

			return GetChartData(data, from, to);
		}
				
		public (DateTime from, DateTime to) GetHouseLimits()
		{
			var data = _dbContext.HouseConsumers
					.Include(p => p.Consumptions)
					.OrderByDescending(c => c.UploadDateTime)
					.FirstOrDefault()
					.Consumptions;
			return (
				from: data.OrderBy(d => d.Date).FirstOrDefault().Date,
				to: data.OrderByDescending(d => d.Date).FirstOrDefault().Date
			);
		}

		public (DateTime from, DateTime to) GetPlantsLimits()
		{
			var data = _dbContext.PlantsConsumers
				.Include(p => p.Consumptions)
				.OrderByDescending(c => c.UploadDateTime)
				.FirstOrDefault()
				.Consumptions;
			return (
				from: data.OrderBy(d => d.Date).FirstOrDefault().Date, 
				to: data.OrderByDescending(d => d.Date).FirstOrDefault().Date
			);
		}
	}
}
