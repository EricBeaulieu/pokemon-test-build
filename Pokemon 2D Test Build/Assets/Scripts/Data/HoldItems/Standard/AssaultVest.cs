using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultVest : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.AssaultVest; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new AssaultVest(); }
    public override bool PreventTheUseOfCertainMoves(BattleUnit battleUnit, MoveBase move)
    {
        if(move.MoveType == MoveType.Status)
        {
            return true;
        }
        return base.PreventTheUseOfCertainMoves(battleUnit,move);
    }
    public override string PreventTheUseOfCertainMoveMessage()
    {
        return "The effects of Assult Vest prevents status moves from being used!";
    }
    public override float AlterStat(StatAttribute statAffected)
    {
        if(statAffected == StatAttribute.SpecialDefense)
        {
            return 1.5f;
        }
        return base.AlterStat(statAffected);
    }
}
