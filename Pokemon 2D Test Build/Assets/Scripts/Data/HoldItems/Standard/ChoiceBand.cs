using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceBand : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.ChoiceBand; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new ChoiceBand(); }
    public override float AlterStat(Pokemon holder,StatAttribute statAffected)
    {
        if(statAffected == StatAttribute.Attack)
        {
            return 1.5f;
        }
        return base.AlterStat(holder, statAffected);
    }
    public override bool PreventTheUseOfCertainMoves(BattleUnit battleUnit, MoveBase move)
    {
        if (battleUnit.lastMoveUsed != null)
        {
            if(battleUnit.lastMoveUsed.moveBase != move)
            {
                return true;
            }
        }
        return false;
    }
}
