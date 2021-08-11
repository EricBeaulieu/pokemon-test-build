using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceScarf : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.ChoiceScarf; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new ChoiceScarf(); }
    MoveBase lockedMove;
    public override float AlterStat(Pokemon holder, StatAttribute statAffected)
    {
        if (statAffected == StatAttribute.Speed)
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
    public override string SpecializedMessage()
    {
        return $"The {GlobalTools.SplitCamelCase(Id.ToString())} only allows the use of {lockedMove.MoveName}";
    }
}
