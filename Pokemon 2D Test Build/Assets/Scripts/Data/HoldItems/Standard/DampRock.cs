using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DampRock : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.DampRock; } }
    public override int IncreasedWeatherEffectDuration(WeatherEffectID currentWeatherEffect)
    {
        if (currentWeatherEffect != WeatherEffectID.NA && currentWeatherEffect == WeatherEffectID.Rain)
        {
            return 3;
        }
        return 0;
    }
}
