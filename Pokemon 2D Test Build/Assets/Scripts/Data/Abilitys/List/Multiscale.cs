using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multiscale : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Multiscale; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Multiscale(); }
    public override string Description()
    {
        return "Reduces the amount of damage the Pokémon takes while its HP is full.";
    }
    public override float AlterDamageTaken(BattleUnit defendingPokemon, MoveBase move, WeatherEffectID weather)
    {
        if (defendingPokemon.pokemon.currentHitPoints == defendingPokemon.pokemon.maxHitPoints)
        {
            return 0.5f;
        }
        return base.AlterDamageTaken(defendingPokemon, move, weather);
    }
}
