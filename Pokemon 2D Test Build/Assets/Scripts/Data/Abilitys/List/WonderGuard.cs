using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WonderGuard : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.WonderGuard; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new WonderGuard(); }
    public override string Description()
    {
        return "Its mysterious power only lets supereffective moves hit the Pokémon.";
    }
    public override float AlterDamageTaken(Pokemon defendingPokemon, MoveBase move, WeatherEffectID weather)
    {
        if (DamageModifiers.TypeChartEffectiveness(defendingPokemon.pokemonBase,move.Type) < 2)
        {
            defendingPokemon.statusChanges.Enqueue($"{Name} prevents damage");
            return 0;
        }
        return base.AlterDamageTaken(defendingPokemon, move, weather);
    }
}
