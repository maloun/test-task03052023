using demo.Views.Charts;
using Microsoft.EntityFrameworkCore;
using demo.Models.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

		public ChartData GetChartsData(DateTime from, DateTime to)
		{
			var houseLastUpload = _dbContext.HouseConsumers.Max(c => c.UploadDateTime);
			var dataHouses = _dbContext.HouseConsumers
				.Include(p => p.Consumptions)
				.Where(c => c.UploadDateTime == houseLastUpload)
				.SelectMany(c => c.Consumptions)
				.Where(d => d.Date >= from && d.Date <= to)
				.OrderBy(d => d.Date)
				.ToList();

			var plantsLastUpload = _dbContext.PlantsConsumers.Max(c => c.UploadDateTime);
			var dataPlants = _dbContext.PlantsConsumers
				.Include(p => p.Consumptions)
				.Where(c => c.UploadDateTime == plantsLastUpload)
				.SelectMany(c => c.Consumptions)
				.Where(d => d.Date >= from && d.Date <= to)
				.OrderBy(d => d.Date)
				.ToList();

			/*
			// сглаживание - скользящее окно, размер окна 15
			var window = 15;
			var smooth = data
				.AverageWindow(window, d => d.y)
				.Zip(
					data.Select(d => d.x.AddDays(window)),
					(a, b) => new TimePoint() { x = b, y = a }
				).ToList();

			// сглаживание - экспоненциальное 	
			double alpha = 0.3
			smooth = smooth.Select((value, index) =>
			{
				if (index == 0)
					return value;
				else
					return new TimePoint()
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
			*/

			var conHouses = _dbContext.HouseConsumptions.Select(d => new TimePoint() { x = d.Date, y = d.Consumption });
			var conPlants = _dbContext.PlantsConsumptions.Select(d => new TimePoint() { x = d.Date, y = d.Consumption });
			var conCity = conHouses.Concat(conPlants).OrderBy(d => d.x).ToList();
						
			var coeffs = CalcLinearRegressionCoefficients(conCity, d => d.x.Ticks, d => d.y);
								
			var sum = conCity.Sum(d => d.y);
			var houses = _dbContext.HouseConsumptions
					.Include(p => p.Consumer)
					.Where(d => d.Date >= from && d.Date <= to)
					.AsEnumerable()
					.GroupBy(d => d.Consumer.Name)
					.Select((g, index) => new NameValue()
					{
						title = g.Key,
						x = index,
						y = g.Sum(x => x.Consumption)
					}).ToList();

			var plants = _dbContext.PlantsConsumptions
					.Include(p => p.Consumer)
					.Where(d => d.Date >= from && d.Date <= to)
					.AsEnumerable()
					.GroupBy(d => d.Consumer.Name)
					.Select((g, index) => new NameValue()
					{
						title = g.Key,
						x = index,
						y = g.Sum(x => x.Consumption)
					}).ToList();

			return new ChartData()
			{
				// линейная зависимость потребления от температуры
				LinearHouses = dataHouses
					.Where(d => d.Date >= from && d.Date <= to)
					.OrderBy(d => d.Weather).Select(d => new ValuePoint()
					{
						x = d.Weather,
						y = d.Consumption
					}),

				// линейная зависимость потребления от цены
				LinearPlants = dataPlants
					.Where(d => d.Date >= from && d.Date <= to)
					.OrderBy(d => d.Price).Select(d => new ValuePoint()
					{
						x = d.Price,
						y = d.Consumption
					}),

				// прогноз потребления города на следующий день
				CityForecast = new TimePoint() {
					x = conCity.Max(d => d.x).AddDays(1),
					y = coeffs.intercept + coeffs.slope * conCity.Max(d => d.x).Ticks
				},

				// график потребления города
				CityConsumptions = new CityConsumption()
				{
					Sum = new ValuePoint[] {
						new ValuePoint(){ x = 0, y =  sum},
						new ValuePoint(){ x = 4, y = sum }
					}.ToList(),
					Houses = houses,
					Plants = plants,
					Pie = houses
					.Select(h => new NameValue() { y = h.y / sum * 100, title = h.title })
					.Concat(
						plants.Select(p => new NameValue() { y = p.y / sum * 100, title = p.title })
					)
				}
			};
		}

		public ChartsLimits GetDefaultLimits()
		{
			var dataHouses = _dbContext.HouseConsumers
					.Include(p => p.Consumptions)
					.OrderByDescending(c => c.UploadDateTime)
					.FirstOrDefault()
					.Consumptions;
			var hMin = dataHouses.OrderBy(d => d.Date).FirstOrDefault().Date;
			var hMax = dataHouses.OrderByDescending(d => d.Date).FirstOrDefault().Date;
			
			var dataPlants = _dbContext.HouseConsumers
					.Include(p => p.Consumptions)
					.OrderByDescending(c => c.UploadDateTime)
					.FirstOrDefault()
					.Consumptions;
			var pMin = dataPlants.OrderBy(d => d.Date).FirstOrDefault().Date;
			var pMax = dataPlants.OrderByDescending(d => d.Date).FirstOrDefault().Date;

			return new ChartsLimits(){
				from = new DateTime(Math.Max(pMin.Ticks, hMin.Ticks)),
				to = new DateTime(Math.Min(pMax.Ticks, hMax.Ticks))
			};
		}
			
	}
}
