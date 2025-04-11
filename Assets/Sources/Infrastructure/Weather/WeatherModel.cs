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
        private const int RequestCooldown = 5;

        private readonly ServerRequestQueue _requestQueue;
        private readonly List<ServerRequest> _requests;

        private float passedTime;

        public WeatherModel(ServerRequestQueue requestQueue)
        {
            _requestQueue = requestQueue;

            _requests = new();
        }

        public event Action<string, string, string> InfoChanged;

        public void Tick()
        {
            passedTime += Time.deltaTime;

            if (passedTime >= RequestCooldown)
            {
                passedTime = 0;
                
                SendRequest().Forget();
            }
        }

        private async UniTaskVoid SendRequest()
        {
            using (UnityWebRequest webRequest =
                   UnityWebRequest.Get("https://api.weather.gov/gridpoints/TOP/32,81/forecast"))
            {
                webRequest.SetRequestHeader("User-Agent", "UnityWeatherApp/1.0 (your@email.com)");
                ServerRequest request = new(webRequest);
                _requests.Add(request);
                
                string jsonResponse = await _requestQueue.EnqueueRequest(request);
                
                Debug.Log("succsess " + jsonResponse);
                
                WeatherForecast forecast = JsonUtility.FromJson<WeatherForecast>(jsonResponse);
                
                if (forecast.properties.periods != null)
                {
                    var period = forecast.properties.periods.Last();
                    
                    InfoChanged?.Invoke(period.name, period.temperature + period.temperatureUnit, period.shortForecast);
                }
            }
        }
        
        [Serializable]
        private struct WeatherForecast
        {
            public WeatherProperties properties;
    
            [Serializable]
            public struct WeatherProperties
            {
                public WeatherPeriod[] periods;
        
                [Serializable]
                public struct WeatherPeriod
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