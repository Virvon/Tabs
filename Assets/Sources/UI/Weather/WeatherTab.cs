using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace Sources.UI.Weather
{
    public class WeatherTab : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _temperatureText;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Sprite _sunSprite;
        [SerializeField] private Sprite _cloudSprite;
        [SerializeField] private Sprite _rainSprite;
        [SerializeField] private Image _icon;
        
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
            _icon.sprite = GetIcon(shortForecast);
        }

        private Sprite GetIcon(string shortForecast)
        {
            string lowerDescription = shortForecast.ToLower();

            if (lowerDescription.Contains("sunny"))
                return _sunSprite;
            else if (lowerDescription.Contains("mostly clear"))
                return _sunSprite;
            else if (lowerDescription.Contains("cloudy"))
                return _cloudSprite;
            else if (lowerDescription.Contains("rain"))
                return _rainSprite;
            else
                return _sunSprite;
        }
    }
}