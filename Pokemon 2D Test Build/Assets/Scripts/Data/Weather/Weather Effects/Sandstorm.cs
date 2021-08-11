using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sandstorm : WeatherEffectBase
{
    public override WeatherEffectID Id { get { return WeatherEffectID.Sandstorm; } }
    public override string StartMessage()
    {
        duration = 5;
        return "A Sandstorm kicked up";
    }
    public override string OnEndTurn()
    {
        duration--;
        if (duration > 0)
        {
            return "The Sandstorm rages";
        }

        BattleSystem.RemoveWeatherEffect();
        return "The Sandstorm subsided.";
    }
    public override void OnEndTurnDamage(Pokemon pokemon)
    {
        if (pokemon.pokemonBase.IsType(ElementType.Rock) || pokemon.pokemonBase.IsType(ElementType.Steel) || pokemon.pokemonBase.IsType(ElementType.Ground))
        {
            return;
        }

        if(pokemon.ability.Id == AbilityID.SandForce)
        {
            return;
        }

        if (pokemon.GetHoldItemEffects.ProtectsHolderFromWeatherConditions() == true)
        {
            return;
        }

        int damage = pokemon.maxHitPoints / 16;

        if (damage <= 0)
        {
            damage = 1;
        }

        pokemon.UpdateHPDamage(damage);
        pokemon.statusChanges.Enqueue($"{pokemon.currentName} was hit by the sandstorm");
    }
}
