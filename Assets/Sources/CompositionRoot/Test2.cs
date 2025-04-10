using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Sources.CompositionRoot
{
    public class Test2 : MonoBehaviour
    {
        
        void Start()
        {
            StartCoroutine(GetWeatherData());
        }

        private IEnumerator GetWeatherData()
        {
            Debug.Log("test 2");
            string url = "https://api.weather.gov/gridpoints/TOP/32,81/forecast";
        
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                webRequest.SetRequestHeader("User-Agent", "YourAppName/1.0 (your@email.com)");
            
                yield return webRequest.SendWebRequest();
            
                if (webRequest.result == UnityWebRequest.Result.ConnectionError || 
                    webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("Error: " + webRequest.error);
                    Debug.LogError("Response: " + webRequest.downloadHandler.text);
                }
                else
                {
                    string jsonResponse = webRequest.downloadHandler.text;
                    Debug.Log("Received: " + jsonResponse);
                
                    ProcessWeatherData(jsonResponse);
                }
            }
        }
        private void ProcessWeatherData(string json)
        {
            WeatherForecast forecast = JsonUtility.FromJson<WeatherForecast>(json);
    
            foreach (WeatherPeriod period in forecast.properties.periods)
            {
                Debug.Log($"Period: {period.name}, Temp: {period.temperature}{period.temperatureUnit}, " +
                          $"Forecast: {period.shortForecast}");
            }
        }
    }
        [System.Serializable]
    public class WeatherForecast
    {
        public WeatherProperties properties;
    }

    [System.Serializable]
    public class WeatherProperties
    {
        public WeatherPeriod[] periods;
    }

    [System.Serializable]
    public class WeatherPeriod
    {
        public int number;
        public string name;
        public string startTime;
        public string endTime;
        public bool isDaytime;
        public int temperature;
        public string temperatureUnit;
        public string windSpeed;
        public string windDirection;
        public string shortForecast;
        public string detailedForecast;
    }

    
}