using System;

namespace Sources.UI.Weather
{
    public interface IWeatherPresetner
    {
        event Action<string, string, string> InfoChanged;
        event Action<bool> ActiveChanged;
    }
}