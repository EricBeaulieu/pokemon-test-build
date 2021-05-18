using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlushRush : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.SlushRush; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new SlushRush(); }
    public override string Description()
    {
        return "Boosts the Pokémon's Speed stat in a hailstorm.";
    }
    public override float DoublesSpeedInAWeatherEffect(WeatherEffectID iD)
    {
        if (iD == WeatherEffectID.Hail)
        {
            return 2;
        }
        return base.DoublesSpeedInAWeatherEffect(iD);
    }
}
