using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeatherEffectID
{
    NA, Sunshine, Rain, Hail, Sandstorm
}

public class WeatherEffectDB
{
    public static void Initialization(List<WeatherEffectBase> weatherBases)
    {
        foreach (WeatherEffectBase weather in weatherBases)
        {
            WeatherEffects.Add(weather.Id, weather);
        }
    }

    public static Dictionary<WeatherEffectID, WeatherEffectBase> WeatherEffects = new Dictionary<WeatherEffectID, WeatherEffectBase>();
}
