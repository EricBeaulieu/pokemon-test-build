using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandRush : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.SandRush; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new SandRush(); }
    public override string Description()
    {
        return "Boosts the Pokémon's Speed stat in a sandstorm.";
    }
    public override float DoublesSpeedInAWeatherEffect(WeatherEffectID iD)
    {
        if (iD == WeatherEffectID.Sandstorm)
        {
            return 2;
        }
        return base.DoublesSpeedInAWeatherEffect(iD);
    }
}
