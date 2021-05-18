using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drizzle : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Drizzle; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Drizzle(); }
    public override string Description()
    {
        return "The Pokémon makes it rain when it enters a battle.";
    }
    public override WeatherEffectID OnStartWeatherEffect()
    {
        return WeatherEffectID.Rain;
    }
}
