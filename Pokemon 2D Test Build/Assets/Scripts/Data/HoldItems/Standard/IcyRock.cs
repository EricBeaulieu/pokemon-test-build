using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcyRock : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.IcyRock; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new IcyRock(); }
    public override int IncreasedWeatherEffectDuration(WeatherEffectID currentWeatherEffect)
    {
        if (currentWeatherEffect != WeatherEffectID.NA && currentWeatherEffect == WeatherEffectID.Hail)
        {
            return 3;
        }
        return 0;
    }
}
