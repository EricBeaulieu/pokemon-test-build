using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBody : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.IceBody; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new IceBody(); }
    public override string Description()
    {
        return "The Pokémon gradually regains HP in a hailstorm.";
    }
    public override bool AffectsHpByXEachTurnWithWeather(Pokemon pokemon, WeatherEffectID weather)
    {
        if (weather != WeatherEffectID.Hail)
        {
            return false;
        }

        if (pokemon.maxHitPoints == pokemon.currentHitPoints)
        {
            return false;
        }

        int hpHealed = Mathf.FloorToInt(pokemon.maxHitPoints * HpAmountHealedByWeather);
        hpHealed = Mathf.Clamp(hpHealed, 1, pokemon.maxHitPoints - pokemon.currentHitPoints);
        pokemon.UpdateHPRestored(hpHealed);
        pokemon.statusChanges.Enqueue($"{pokemon.currentName} restored HP with Ice Body!");

        return true;
    }
}
