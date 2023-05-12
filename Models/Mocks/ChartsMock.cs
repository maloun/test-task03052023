using demo.Views.Charts;
using Microsoft.EntityFrameworkCore;
using demo.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
			try
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
			catch(Exception e)
			{
				throw new Exception($"Исключение в CalcLinearRegressionCoefficients: {e.Message}");
			}
		}

		public ChartData GetChartsData(DateTime from, DateTime to)
		{
			try
			{ 
				// получаем последние данные потребления по домам, по фильтру в список ChartPoint 
				var houseLastUpload = _dbContext.HouseConsumers.Max(c => c.UploadDateTime);
				var dataHouses = _dbContext.HouseConsumptions
					.Include(h => h.Consumer)
					.Where(d => d.Consumer.UploadDateTime == houseLastUpload &&
						   d.Date >= from && d.Date <= to)
					.AsEnumerable().Select(d => ChartPoint.FromHouse(d));

				// получаем последние данные потребления по заводам, по фильтру в список ChartPoint 
				var plantsLastUpload = _dbContext.PlantsConsumers.Max(c => c.UploadDateTime);
				var dataPlants = _dbContext.PlantsConsumptions
					.Include(h => h.Consumer)
					.Where(d => d.Consumer.UploadDateTime == houseLastUpload &&
						   d.Date >= from && d.Date <= to)
					.AsEnumerable().Select(d => ChartPoint.FromPlants(d));

				// получаем данные по городу, объединением заводов и домов  
				var dataCity = dataHouses.ToArray().Concat(dataPlants.ToArray()).OrderBy(d => d.date);
			
				// считаем коофиценты для линейной регрессии по городу из даты и потребления
				var coeffs = CalcLinearRegressionCoefficients(dataCity.ToArray(), d => d.date.Ticks, d => d.y);
								
				// считаем сумму тепла по городу, группируем дома и заводы по имени и сумме показаний
				var sum = dataCity.Sum(d => d.y);
				var houses = dataHouses.GroupBy(d => d.title)
					.Select((g, i) => new ChartPoint() { title = g.Key, x = i, y = g.Sum(v => v.y) });
				var plants = dataPlants.GroupBy(d => d.title)
					.Select((g, i) => new ChartPoint() { title = g.Key, x = i, y = g.Sum(v => v.y) });

				return new ChartData()
				{
					// линейная зависимость потребления от температуры для домов
					LinearHouses = dataHouses.OrderBy(d => d.x),

					// линейная зависимость потребления от цены для заводов
					LinearPlants = dataPlants.OrderBy(d => d.x),

					// прогноз потребления города на следующий день
					CityForecast = new ChartPoint() {
						date = dataCity.Max(d => d.date).AddDays(1),
						y = coeffs.intercept + coeffs.slope * dataCity.Max(d => d.date).Ticks
					},

					// график потребления города
					CityConsumptions = new CityConsumption()
					{
						Sum = houses.Select((v,i) => new ChartPoint { x = i, y = sum }),
						Houses = houses.ToList(),
						Plants = plants.ToList(),
						Pie = houses
							.Select(h => new ChartPoint() { y = h.y / sum * 100, title = h.title })
							.Concat(plants.Select(p => new ChartPoint() { y = p.y / sum * 100, title = p.title }))
					}
				};
			}
			catch (Exception e)
			{
				throw new Exception($"Исключение в GetChartsData: {e.Message}");
			}
		}

		public ChartsLimits GetDefaultLimits()
		{
			try
			{
				var dataHouses = _dbContext.HouseConsumers
					.Include(p => p.Consumptions)
					.OrderByDescending(c => c.UploadDateTime)
					.FirstOrDefault()?
					.Consumptions;
				var hMin = dataHouses?.OrderBy(d => d.Date).FirstOrDefault()?.Date ?? DateTime.MinValue;
				var hMax = dataHouses?.OrderByDescending(d => d.Date).FirstOrDefault()?.Date ?? DateTime.MaxValue;

				var dataPlants = _dbContext?.HouseConsumers
						.Include(p => p.Consumptions)
						.OrderByDescending(c => c.UploadDateTime)
						.FirstOrDefault()?
						.Consumptions;
				var pMin = dataPlants?.OrderBy(d => d.Date).FirstOrDefault()?.Date ?? DateTime.MinValue;
				var pMax = dataPlants?.OrderByDescending(d => d.Date).FirstOrDefault()?.Date ?? DateTime.MaxValue;

				return new ChartsLimits()
				{
					from = new DateTime(Math.Max(pMin.Ticks, hMin.Ticks)),
					to = new DateTime(Math.Min(pMax.Ticks, hMax.Ticks))
				};
			}
			catch(Exception e)
			{
				throw new Exception($"Исключение в GetDefaultLimits: {e.Message}");
			}			
		}			
	}
}
