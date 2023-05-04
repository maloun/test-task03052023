using demo.Views.Charts;
using Microsoft.EntityFrameworkCore;
using demo.Models.Interfaces;

namespace demo.Models.Mocks
{
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

		public ChartData GetHousesData(DateTime from, DateTime to) 
		{
			var data = _dbContext.HouseConsumers
				.Include(h => h.Consumptions)
				.OrderByDescending(c => c.UploadDateTime)
				.FirstOrDefault()
				.Consumptions
				.Where(d => d.Date >= from && d.Date <= to)
				.ToList();

			if (data.Count == 0)
				throw new Exception("данные не найдены");

			var coeffs = CalcLinearRegressionCoefficients(data, d => d.Weather, d => d.Date.Ticks);

			return new ChartData()
			{
				Data = from d in data select new ChartPoint() { x = d.Date, y = d.Weather},

				Forecast = from d in data select new ChartPoint() {
					x = d.Date,
					y = coeffs.intercept + coeffs.slope * d.Date.Ticks
				},

				Linear = new Linear()
				{
					st = new ChartPoint()
					{
						x = data.Min(dp => dp.Date),
						y = coeffs.intercept + coeffs.slope * data.Min(dp => dp.Date).Ticks
					},
					en = new ChartPoint()
					{
						x = data.Max(dp => dp.Date),
						y = coeffs.intercept + coeffs.slope * data.Max(dp => dp.Date).Ticks
					}
				}
			};

			/*
			var result = new AxisData();
			var consumptions = _dbContext.HouseConsumers
				.Include(h => h.Consumptions)
				.OrderByDescending(c => c.UploadDateTime)
				.FirstOrDefault()
				.Consumptions
				.ToList();

			double sum = 0;
			result.Period = consumptions.Count;
			var window = 3;

			for (int i = 0; i < result.Period; i++)
			{
				var point = Tuple.Create(				
					consumptions[i].Date,
					consumptions[i].Weather
				);

				if (i >= window)
				{
					point.AxisZ = (consumptions[i].Weather - result.Data[0].AxisZ) / window;
				}
				else
				{
					sum += consumptions[i].Weather;
					point.AxisZ = sum / (result.Data.Count + 1);
				}
				result.Data.Add(point);
			}

			return result;
			*/
			throw new NotImplementedException();
		}

		public ChartData GetPlantsData(DateTime from, DateTime to) 
		{
			var data = _dbContext.PlantsConsumers
				.Include(p => p.Consumptions)
				.OrderByDescending(c => c.UploadDateTime)
				.FirstOrDefault()
				.Consumptions
				.Where(d => d.Date > from && d.Date < to)
				.ToList();

			var coeffs = CalcLinearRegressionCoefficients(data, d => d.Price, d => d.Date.Ticks);

			return new ChartData()
			{
				Data = from d in data select new ChartPoint() { x = d.Date, y = d.Price },

				Forecast = from d in data
						   select new ChartPoint()
						   {
							   x = d.Date,
							   y = coeffs.intercept + coeffs.slope * d.Date.Ticks
						   },

				Linear = new Linear()
				{
					st = new ChartPoint()
					{
						x = data.Min(dp => dp.Date),
						y = coeffs.intercept + coeffs.slope * data.Min(dp => dp.Date).Ticks
					},
					en = new ChartPoint()
					{
						x = data.Max(dp => dp.Date),
						y = coeffs.intercept + coeffs.slope * data.Max(dp => dp.Date).Ticks
					}
				}
			};
			

			/*
			//List<double> prices = dataPoints.Select(d => d.Price).ToList();
			//List<double> dates = dataPoints.Select(d => (double)d.Date.Ticks).ToList();

			//var linearRegression = CalcLinRegCoeffs(prices, dates);
			//var slope = linearRegression.Item1;
			//var intercept = linearRegression.Item2;
			*/


			/*
			// вычисляем коэффициенты линейной регрессии
			double avgDate = dataPoints.Average(dp => dp.Date.Ticks);
			double avgPrice = dataPoints.Average(dp => dp.Price);
			double sumXY = dataPoints.Sum(dp => (dp.Date.Ticks - avgDate) * (dp.Price - avgPrice));
			double sumXX = dataPoints.Sum(dp => (dp.Date.Ticks - avgDate) * (dp.Date.Ticks - avgDate));
			double slope = sumXY / sumXX;
			double intercept = avgPrice - slope * avgDate;

			// линейный тренд
			seriesTrend.Points.AddXY(dataPoints.Min(dp => dp.Date), intercept + slope * dataPoints.Min(dp => dp.Date).Ticks);
			seriesTrend.Points.AddXY(dataPoints.Max(dp => dp.Date), intercept + slope * dataPoints.Max(dp => dp.Date).Ticks);
			
			// создаем список данных для прогноза
			List<DataPoint> forecastPoints = new List<DataPoint>
			{
				new DataPoint { Date = new DateTime(2021, 6, 1) },
				new DataPoint { Date = new DateTime(2021, 7, 1) },
				new DataPoint { Date = new DateTime(2021, 8, 1) },
				new DataPoint { Date = new DateTime(2021, 9, 1) },
				new DataPoint { Date = new DateTime(2021, 10, 1) },
			};

			// добавляем прогнозные значения
			foreach (var fp in forecastPoints)
			{
				fp.Price = intercept + slope * fp.Date.Ticks;
			}
			*/

			/*
			var result = new AxisData();
			var consumptions = _dbContext.PlantsConsumers
				.Include(h => h.Consumptions)
				.OrderByDescending(c => c.UploadDateTime)
				.FirstOrDefault()
				.Consumptions
				.ToList();

			double sum = 0;
			result.Period = consumptions.Count;
			var window = 3;

			for (int i = 0; i < result.Period; i++)
			{
				var point = new ChartPoint()
				{
					AxisX = consumptions[i].Date,
					AxisY = consumptions[i].Price
				};

				if (i >= window)
					point.AxisZ = (consumptions[i].Price - result.Data[0].AxisZ) / window;
				else
				{
					sum += consumptions[i].Price;
					point.AxisZ = sum / (result.Data.Count + 1);
				}
				result.Data.Add(point);
			}

			return result;
			*/
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
