using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adaptability : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Adaptability; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Adaptability(); }
    public override string Description()
    {
        return "Powers up moves of the same type as the Pokémon.";
    }
    public override float PowerUpCertainMoves(Pokemon attackingPokemon, BattleUnit defendingPokemon, MoveBase currentMove, WeatherEffectID weather)
    {
        if (attackingPokemon.pokemonBase.IsType(currentMove.Type) == true)
        {
            return 1.33f;
        }
        return base.PowerUpCertainMoves(attackingPokemon, defendingPokemon, currentMove,weather);
    }
}
