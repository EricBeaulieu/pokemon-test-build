using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwiftSwim : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.SwiftSwim; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new SwiftSwim(); }
    public override string Description()
    {
        return "Boosts the Pokémon's Speed stat in rain.";
    }
    public override float DoublesSpeedInAWeatherEffect(WeatherEffectID iD)
    {
        if (iD == WeatherEffectID.Rain)
        {
            return 2;
        }
        return base.DoublesSpeedInAWeatherEffect(iD);
    }
}
