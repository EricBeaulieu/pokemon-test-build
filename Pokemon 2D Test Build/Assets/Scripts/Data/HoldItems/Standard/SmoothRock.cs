using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothRock : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.SmoothRock; } }
    public override int IncreasedWeatherEffectDuration(WeatherEffectID currentWeatherEffect)
    {
        if (currentWeatherEffect != WeatherEffectID.NA && currentWeatherEffect == WeatherEffectID.Sandstorm)
        {
            return 3;
        }
        return 0;
    }
}
