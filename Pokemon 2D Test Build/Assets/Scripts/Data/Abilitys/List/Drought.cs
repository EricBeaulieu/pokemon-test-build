using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drought : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Drought; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Drought(); }
    public override string Description()
    {
        return "Turns the sunlight harsh when the Pokémon enters a battle.";
    }
    public override WeatherEffectID OnStartWeatherEffect()
    {
        return WeatherEffectID.Sunshine;
    }
}
