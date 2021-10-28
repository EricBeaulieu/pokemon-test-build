using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToughClaws : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.ToughClaws; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new ToughClaws(); }
    public override string Description()
    {
        return "Powers up moves that make direct contact.";
    }
    public override float PowerUpCertainMoves(Pokemon attackingPokemon, BattleUnit defendingPokemon, MoveBase currentMove, WeatherEffectID weather)
    {
        if (currentMove.PhysicalContact == true)
        {
            return 1.3f;
        }
        return base.PowerUpCertainMoves(attackingPokemon, defendingPokemon, currentMove,weather);
    }
}
