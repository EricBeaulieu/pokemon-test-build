using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hail : WeatherEffectBase
{
    public override WeatherEffectID Id { get { return WeatherEffectID.Hail; } }
    public override string StartMessage()
    {
        duration = 5;
        return "It started to Hail";
    }
    public override string OnEndTurn(BattleSystem battleSystem)
    {
        duration--;
        if (duration > 0)
        {
            return "It continues to Hail";
        }

        battleSystem.RemoveWeatherEffect();
        return "The hail stopped.";
    }
    public override void OnEndTurnDamage(Pokemon pokemon)
    {
        if (pokemon.pokemonBase.IsType(ElementType.Ice))
        {
            return;
        }

        int damage = pokemon.maxHitPoints / 16;

        if (damage <= 0)
        {
            damage = 1;
        }

        pokemon.UpdateHPDamage(damage);
        pokemon.statusChanges.Enqueue($"{pokemon.currentName} was hit by hail");
    }
}