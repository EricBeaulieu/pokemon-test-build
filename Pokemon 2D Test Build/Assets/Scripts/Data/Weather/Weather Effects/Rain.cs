using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rain : WeatherEffectBase
{
    public override WeatherEffectID Id { get { return WeatherEffectID.Rain; } }
    public override string StartMessage()
    {
        duration = 5;
        return "It started to Rain";
    }
    public override string OnEndTurn(BattleSystem battleSystem)
    {
        duration--;
        if (duration > 0)
        {
            return "It continues to Rain";
        }

        battleSystem.RemoveWeatherEffect();
        return "The Rain Stopped.";
    }
}
