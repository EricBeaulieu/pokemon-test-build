using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandStream : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.SandStream; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new SandStream(); }
    public override string Description()
    {
        return "Boosts the Pokémon's Speed stat in a sandstorm.";
    }
    public override WeatherEffectID OnStartWeatherEffect()
    {
        return WeatherEffectID.Sandstorm;
    }
}
