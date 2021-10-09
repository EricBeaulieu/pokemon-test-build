using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultVest : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.AssaultVest; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new AssaultVest(); }
    public override bool PreventTheUseOfCertainMoves(BattleUnit battleUnit, MoveBase move)
    {
        if(move.MoveType == MoveType.Status)
        {
            return true;
        }
        return base.PreventTheUseOfCertainMoves(battleUnit,move);
    }
    public override string SpecializedMessage(Pokemon holder,Pokemon opposingPokemon)
    {
        return $"The effects of {GlobalTools.SplitCamelCase(HoldItemId.ToString())} prevents status moves from being used!";
    }
    public override float AlterStat(Pokemon holder, StatAttribute statAffected)
    {
        if(statAffected == StatAttribute.SpecialDefense)
        {
            return 1.5f;
        }
        return base.AlterStat(holder,statAffected);
    }
}
