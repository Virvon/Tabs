using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sources.Infrastructure.ServerRequests;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

namespace Sources.Infrastructure.Weather
{
    public class WeatherModel : ITickable
    {
        private const string URL = "https://api.weather.gov/gridpoints/TOP/32,81/forecast";
        
        private const int RequestCooldown = 5;

        private readonly ServerRequestQueue _requestQueue;
        private readonly List<ServerRequest> _requests;

        private float passedTime;
        private bool _isWork;

        public WeatherModel(ServerRequestQueue requestQueue)
        {
            _requestQueue = requestQueue;

            _requests = new();
            _isWork = false;
        }

        public event Action<string, string, string> InfoChanged;

        public void Tick()
        {
            if(_isWork == false)
                return;
            
            passedTime += Time.deltaTime;

            if (passedTime >= RequestCooldown)
            {
                passedTime = 0;
                
                SendRequest().Forget();
            }
        }
        
        public void UpdateInfo()
        {
            passedTime = RequestCooldown;
            _isWork = true;
        }

        public void StopUpdate()
        {
            _isWork = false;

            foreach (ServerRequest request in _requests)
                _requestQueue.CancelRequest(request);
            
            _requests.Clear();
        }

        private async UniTaskVoid SendRequest()
        {
            using (UnityWebRequest webRequest =
                   UnityWebRequest.Get(URL))
            {
                webRequest.SetRequestHeader("User-Agent", "UnityWeatherApp/1.0 (your@email.com)");
                ServerRequest request = new(webRequest);
                _requests.Add(request);
                
                string jsonResponse = await _requestQueue.EnqueueRequest(request);
                _requests.Remove(request);
                
                WeatherForecast forecast = JsonUtility.FromJson<WeatherForecast>(jsonResponse);

                if (forecast.properties != null && forecast.properties.periods != null)
                {
                    var period = forecast.properties.periods.Last();
                    
                    InfoChanged?.Invoke(period.name, period.temperature + period.temperatureUnit, period.shortForecast);
                }
            }
        }
        
        [Serializable]
        private class WeatherForecast
        {
            public WeatherProperties properties;
    
            [Serializable]
            public class WeatherProperties
            {
                public WeatherPeriod[] periods;
        
                [Serializable]
                public class WeatherPeriod
                {
                    public string name;
                    public int temperature;
                    public string temperatureUnit;
                    public string shortForecast;
                }
            }
        }
    }
}