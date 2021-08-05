using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatRock : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.HeatRock; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new HeatRock(); }
    public override int IncreasedWeatherEffectDuration(WeatherEffectID currentWeatherEffect)
    {
        if (currentWeatherEffect != WeatherEffectID.NA && currentWeatherEffect == WeatherEffectID.Sunshine)
        {
            return 3;
        }
        return 0;
    }
}
