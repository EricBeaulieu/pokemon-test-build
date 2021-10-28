using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transistor : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Transistor; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Transistor(); }
    public override string Description()
    {
        return "Powers up Electric-type moves.";
    }
    public override float PowerUpCertainMoves(Pokemon attackingPokemon, BattleUnit defendingPokemon, MoveBase currentMove, WeatherEffectID weather)
    {
        if (currentMove.Type == ElementType.Electric)
        {
            return 1.5f;
        }
        return base.PowerUpCertainMoves(attackingPokemon, defendingPokemon, currentMove,weather);
    }
}
