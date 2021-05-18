using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowWarning : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.SnowWarning; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new SnowWarning(); }
    public override string Description()
    {
        return "The Pokémon summons a hailstorm when it enters a battle.";
    }
    public override WeatherEffectID OnStartWeatherEffect()
    {
        return WeatherEffectID.Hail;
    }
}
