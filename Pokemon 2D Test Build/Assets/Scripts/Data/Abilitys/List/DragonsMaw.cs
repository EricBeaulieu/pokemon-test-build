using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonsMaw : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.DragonsMaw; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new DragonsMaw(); }
    public override string Description()
    {
        return "Powers up Dragon-type moves.";
    }
    public override float PowerUpCertainMoves(Pokemon attackingPokemon, BattleUnit defendingPokemon, MoveBase currentMove, WeatherEffectID weather)
    {
        if (currentMove.Type == ElementType.Dragon)
        {
            return 1.5f;
        }
        return base.PowerUpCertainMoves(attackingPokemon, defendingPokemon, currentMove,weather);
    }
}
