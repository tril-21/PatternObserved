using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PatternObserved
{
    class Program
    {
        public interface IObservable
        {
            void AddObserver(IObserver o);
            void RemoveObserver(IObserver o);
            void NotifyObserver();
        }
        public interface IObserver
        {
            void Update(WeatherData data);
        }
        public interface IDisposable
        {
            void Dispose();
        }
        public class WeatherDataProvider : IObservable
        {
            List<IObserver> ListOfObservers;
            WeatherData data;
            public WeatherDataProvider()
            {
                ListOfObservers = new List<IObserver>();
            }
            public void AddObserver(IObserver observer)
            {
                ListOfObservers.Add(observer);
            }
            public void RemoveObserver(IObserver observer)
            {
                ListOfObservers.Remove(observer);
            }
            public void NotifyObserver()
            {
                foreach(var ob in ListOfObservers)
                {
                    ob.Update(data);
                }
            }
            private void ChangedData()
            {
                NotifyObserver();
            }
            public void SetWeatherData(double temp, double humid, double pres)
            {
                data = new WeatherData(temp, humid, pres);
                ChangedData();
            }

        }
        public class CurrentDisplay : IObserver
        {
            WeatherData data;
            IObservable WeatherProvider;

            public CurrentDisplay(IObservable wData)
            {
                WeatherProvider = wData;
                WeatherProvider.AddObserver(this);
            }
            public void Display()
            {
                Console.WriteLine("Текущие условия : Температура = {0} С | Влажность = {1}% | Давление = {2} бар", data.Temperature, data.Humidity, data.Pressure);
            }
            public void Update(WeatherData data)
            {
                this.data = data;
                Display();
            }
        }
        public class ForecastDisplay:IObserver, IDisposable
        {
            WeatherData data;
            IObservable WeatherProvider;

            public ForecastDisplay(IObservable wData)
            {
                WeatherProvider = wData;
                WeatherProvider.AddObserver(this);
            }

            public void Display()
            {
                Console.WriteLine("Прогноз : Температура = {0} С | Влажность = {1}% | Давление = {2} бар", data.Temperature+7, data.Humidity+19, data.Pressure-4);
            }
            public void Update(WeatherData data)
            {
                this.data = data;
                Display();
            }
            public void Dispose()
            {
                WeatherProvider.RemoveObserver(this);
            }
        }
        public class WeatherData
        {
            public double Temperature { get; set; }
            public double Humidity { get; set; }
            public double Pressure { get; set; }

            public WeatherData(double temp, double humid, double pres)
            {
                Temperature = temp;
                Humidity = humid;
                Pressure = pres;
            }
        }
        static void Main(string[] args)
        {
            WeatherDataProvider weatherData = new WeatherDataProvider();
            CurrentDisplay cirrentDisp = new CurrentDisplay(weatherData);
            ForecastDisplay forecastDisp = new ForecastDisplay(weatherData);
            weatherData.SetWeatherData(34, 46, 7);
            Console.WriteLine();
            weatherData.SetWeatherData(55, 78, 3);
            Console.WriteLine();
            weatherData.SetWeatherData(40, 67, 8);

            forecastDisp.Dispose();
            Console.WriteLine();
            Console.WriteLine("Прогноз удален!");
            Console.WriteLine();
            weatherData.SetWeatherData(33, 65, 7);

            Console.ReadKey();

        }
    }
}
