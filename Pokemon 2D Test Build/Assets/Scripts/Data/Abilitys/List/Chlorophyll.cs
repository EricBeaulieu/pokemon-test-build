using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chlorophyll : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Chlorophyll; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Chlorophyll(); }
    public override string Description()
    {
        return "Boosts the Pokémon's Speed stat in sunshine.";
    }
    public override float DoublesSpeedInAWeatherEffect(WeatherEffectID iD)
    {
        if (iD == WeatherEffectID.Sunshine)
        {
            return 2;
        }
        return base.DoublesSpeedInAWeatherEffect(iD);
    }
}
