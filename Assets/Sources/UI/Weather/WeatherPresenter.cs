using System;
using Sources.Infrastructure.Weather;
using Sources.UI.TabsMediator;
using UnityEngine.UI;
using Zenject;

namespace Sources.UI.Weather
{
    public class WeatherPresenter : TabPresenter, IWeatherPresetner, IInitializable, IDisposable
    {
        private readonly WeatherModel _weatherModel;

        public WeatherPresenter(Button button, WeatherModel weatherModel)
            : base(button) =>
            _weatherModel = weatherModel;

        public event Action<string, string, string> InfoChanged;
        public event Action<bool> ActiveChanged; 

        public void Initialize()
        {
            _weatherModel.InfoChanged += OnInfoChanged;
            Button.onClick.AddListener(OnButtonClicked);
        }

        public void Dispose()
        {
            _weatherModel.InfoChanged -= InfoChanged;
            Button.onClick.RemoveListener(OnButtonClicked);
            ActiveChanged?.Invoke(false);
        }
        
        public override void ShowTab()
        {
            Button.interactable = false;
            ActiveChanged?.Invoke(true);
        }

        public override void HideTab()
        {
            Button.interactable = true;
            ActiveChanged?.Invoke(false);
        }

        private void OnInfoChanged(string name, string temperature, string shortForecast) =>
            InfoChanged?.Invoke(name, temperature, shortForecast);

        private void OnButtonClicked() =>
            Mediator.Notify(this);
    }
}