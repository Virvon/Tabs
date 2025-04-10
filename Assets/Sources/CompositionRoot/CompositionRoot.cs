using System;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;
using Sources.Infrastructure.ServerRequests;

public class CompositionRoot : MonoBehaviour
{
    private ServerRequestQueue _serverRequestQueue;

    [Inject]
    private void Construct(ServerRequestQueue serverRequestQueue)
    {
        _serverRequestQueue = serverRequestQueue;
    }

    private async void Start()
    {
        Debug.Log("start");
        try
        {
            var webRequest = UnityWebRequest.Get( "https://api.weather.gov/gridpoints/TOP/32,81/forecast");
            var webRequest2 = UnityWebRequest.Get( "https://api.weather.gov/gridpoints/TOP/32,81/forecast");
            webRequest.SetRequestHeader("User-Agent", "UnityWeatherApp/1.0 (your@email.com)");
            webRequest2.SetRequestHeader("User-Agent", "UnityWeatherApp/1.0 (your@email.com)");
            
            ServerRequest serverRequest = new(webRequest);
            ServerRequest serverRequest2 = new(webRequest);
            _serverRequestQueue.EnqueueRequest(serverRequest2);
            string jsonResponse = await _serverRequestQueue.EnqueueRequest(serverRequest);
            _serverRequestQueue.CancelRequest(serverRequest);
            _serverRequestQueue.CancelRequest(serverRequest2);
            
            
            var weatherResponse = JsonUtility.FromJson<WeatherApiResponse>(jsonResponse);
            
            var forecastPeriods = new WeatherData.WeatherPeriod[weatherResponse.properties.periods.Length];
            for (int i = 0; i < weatherResponse.properties.periods.Length; i++)
            {
                forecastPeriods[i] = new WeatherData.WeatherPeriod(
                    weatherResponse.properties.periods[i].name,
                    weatherResponse.properties.periods[i].temperature,
                    weatherResponse.properties.periods[i].shortForecast
                );
            }
            Debug.Log("succsess " + jsonResponse);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to get weather: {e.Message}");
        }
    }
  
}
[System.Serializable]
public class WeatherApiResponse
{
    public WeatherProperties properties;
    
    [System.Serializable]
    public class WeatherProperties
    {
        public Period[] periods;
    }

    [System.Serializable]
    public class Period
    {
        public string name;
        public int temperature;
        public string shortForecast;
    }
}

public class WeatherData
{
    public WeatherPeriod[] ForecastPeriods { get; }
    
    public WeatherData(WeatherPeriod[] periods)
    {
        ForecastPeriods = periods;
    }
    
    public class WeatherPeriod
    {
        public string Name { get; }
        public int Temperature { get; }
        public string Description { get; }
        
        public WeatherPeriod(string name, int temperature, string description)
        {
            Name = name;
            Temperature = temperature;
            Description = description;
        }
    }
}
