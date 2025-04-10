using System;
using Sources.Infrastructure.Weather;
using Zenject;

namespace Sources.UI.Weather
{
    public class WeatherPresenter : IWeatherPresetner, IInitializable, IDisposable
    {
        private readonly WeatherModel _weatherModel;

        public WeatherPresenter(WeatherModel weatherModel)
        {
            _weatherModel = weatherModel;
        }

        public event Action<string, string, string> InfoChanged;

        public void Initialize()
        {
            _weatherModel.InfoChanged += OnInfoChanged;
        }

        public void Dispose()
        {
            _weatherModel.InfoChanged -= InfoChanged;
        }

        private void OnInfoChanged(string name, string temperature, string shortForecast) =>
            InfoChanged?.Invoke(name, temperature, shortForecast);
    }
}