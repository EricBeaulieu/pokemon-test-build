using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steelworker : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Steelworker; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Steelworker(); }
    public override string Description()
    {
        return "Powers up Steel-type moves.";
    }
    public override float PowerUpCertainMoves(Pokemon attackingPokemon, Pokemon defendingPokemon, MoveBase currentMove, WeatherEffectID weather)
    {
        if (currentMove.Type == ElementType.Steel)
        {
            return 1.5f;
        }
        return base.PowerUpCertainMoves(attackingPokemon, defendingPokemon, currentMove,weather);
    }
}
