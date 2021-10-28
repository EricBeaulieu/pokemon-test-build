using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronFist : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.IronFist; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new IronFist(); }
    public override string Description()
    {
        return "Powers up punching moves.";
    }
    public override float PowerUpCertainMoves(Pokemon attackingPokemon, BattleUnit defendingPokemon, MoveBase currentMove, WeatherEffectID weather)
    {
        if (currentMove.PunchMove == true)
        {
            return 1.2f;
        }
        return base.PowerUpCertainMoves(attackingPokemon, defendingPokemon, currentMove,weather);
    }
}
