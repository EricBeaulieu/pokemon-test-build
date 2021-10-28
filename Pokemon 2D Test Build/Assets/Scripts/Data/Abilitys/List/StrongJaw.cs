using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongJaw : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.StrongJaw; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new StrongJaw(); }
    public override string Description()
    {
        return "The Pokémon's strong jaw boosts the power of its biting moves.";
    }
    public override float PowerUpCertainMoves(Pokemon attackingPokemon, BattleUnit defendingPokemon, MoveBase currentMove,WeatherEffectID weather)
    {
        if (currentMove.BitingMove == true)
        {
            return 1.5f;
        }
        return base.PowerUpCertainMoves(attackingPokemon, defendingPokemon, currentMove,weather);
    }
}
