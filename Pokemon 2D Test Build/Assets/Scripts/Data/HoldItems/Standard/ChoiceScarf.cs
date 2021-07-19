using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceScarf : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.ChoiceScarf; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new ChoiceScarf(); }
    public override float AlterStat(StatAttribute statAffected)
    {
        if (statAffected == StatAttribute.Speed)
        {
            return 1.5f;
        }
        return base.AlterStat(statAffected);
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
        return false;
    }
}
