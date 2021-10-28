using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reckless : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Reckless; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Reckless(); }
    public override string Description()
    {
        return "Powers up moves that have recoil damage.";
    }
    public override float PowerUpCertainMoves(Pokemon attackingPokemon, BattleUnit defendingPokemon, MoveBase currentMove, WeatherEffectID weather)
    {
        if (currentMove.RecoilType != Recoil.NA)
        {
            return 1.2f;
        }
        return base.PowerUpCertainMoves(attackingPokemon, defendingPokemon, currentMove,weather);
    }
}
