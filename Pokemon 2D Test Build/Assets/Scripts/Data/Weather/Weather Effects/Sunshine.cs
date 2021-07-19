using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sunshine : WeatherEffectBase
{
    public override WeatherEffectID Id { get { return WeatherEffectID.Sunshine; } }
    public override string StartMessage()
    {
        duration = 5;
        return "The Sunlight turned harsh!";
    }
    public override string OnEndTurn()
    {
        duration--;
        if (duration > 0)
        {
            return "The Sunlight is strong";
        }

        BattleSystem.RemoveWeatherEffect();
        return "The sunlight Faded.";
    }
}
