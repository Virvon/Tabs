using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace Sources.UI.Weather
{
    public class WeatherTab : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _temperatureText;
        [SerializeField] private Canvas _canvas;
        
        private IWeatherPresetner _weatherPresenter;

        [Inject]
        private void Construct(IWeatherPresetner weatherPresetner)
        {
            _weatherPresenter = weatherPresetner;
            _weatherPresenter.ActiveChanged += OnActiveChanged;
        }

        private void OnDestroy() =>
            _weatherPresenter.ActiveChanged -= OnActiveChanged;

        private void OnActiveChanged(bool isActive)
        {
            if(isActive)
                Show();
            else
                Hide();
        }
        
        private void Show()
        {
            _canvas.enabled = true;

            _weatherPresenter.InfoChanged += OnInfoChanged;
        }

        private void Hide()
        {
            _canvas.enabled = false;

            _weatherPresenter.InfoChanged -= OnInfoChanged;
        }

        private void OnInfoChanged(string name, string temperature, string shortForecast)
        {
            _nameText.text = name;
            _temperatureText.text = temperature;
        }
    }
}