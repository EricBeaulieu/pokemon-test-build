using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Analytic : AbilityBase
{
    public override AbilityID Id { get { return AbilityID.Analytic; } }
    public override AbilityBase ReturnDerivedClassAsNew() { return new Analytic(); }
    public override string Description()
    {
        return "Boosts move power when the Pokémon moves last.";
    }
    public override MoveBase BoostsMovePowerWhenLast(bool isLast, MoveBase move)
    {
        if(isLast == true)
        {
            move = move.Clone();
            move.AdjustedMovePower(0.3f);
        }

        return base.BoostsMovePowerWhenLast(isLast, move);
    }
}
