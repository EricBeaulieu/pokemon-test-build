using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceSpecs : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.ChoiceSpecs; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new ChoiceSpecs(); }
    MoveBase lockedMove;
    public override float AlterStat(Pokemon holder, StatAttribute statAffected)
    {
        if (statAffected == StatAttribute.SpecialAttack)
        {
            return 1.5f;
        }
        return base.AlterStat(holder, statAffected);
    }
    public override bool PreventTheUseOfCertainMoves(BattleUnit battleUnit, MoveBase move)
    {
        if (battleUnit.lastMoveUsed != null)
        {
            if (battleUnit.lastMoveUsed.moveBase != move)
            {
                return true;
            }
        }
        lockedMove = move;
        return false;
    }
    public override string PreventTheUseOfCertainMoveMessage()
    {
        return $"The {GlobalTools.SplitCamelCase(Id.ToString())} only allows the use of {lockedMove.MoveName}";
    }
}
