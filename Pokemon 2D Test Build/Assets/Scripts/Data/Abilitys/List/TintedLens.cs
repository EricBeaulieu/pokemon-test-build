using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TintedLens : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.TintedLens; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new TintedLens(); }
    public override string Description()
    {
        return "The Pokémon can use \"not very effective\" moves to deal regular damage.";
    }
    public override float PowerUpCertainMoves(Pokemon attackingPokemon, BattleUnit defendingPokemon, MoveBase currentMove, WeatherEffectID weather)
    {
        if (DamageModifiers.TypeChartEffectiveness(defendingPokemon, currentMove.Type) < 1)
        {
            return 2f;
        }
        return base.PowerUpCertainMoves(attackingPokemon, defendingPokemon, currentMove,weather);
    }
}
