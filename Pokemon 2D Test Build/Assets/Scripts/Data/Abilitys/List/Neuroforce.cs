using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neuroforce : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Neuroforce; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Neuroforce(); }
    public override string Description()
    {
        return "Powers up moves that are super effective.";
    }
    public override float PowerUpCertainMoves(Pokemon attackingPokemon, Pokemon defendingPokemon, MoveBase currentMove, WeatherEffectID weather)
    {
        if (DamageModifiers.TypeChartEffectiveness(defendingPokemon.pokemonBase, currentMove.Type) > 1)
        {
            return 1.25f;
        }
        return base.PowerUpCertainMoves(attackingPokemon, defendingPokemon, currentMove,weather);
    }
}
