using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBracer : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.PowerBracer; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new PowerBracer(); }
    public override float AlterStat(Pokemon Holder, StatAttribute statAffected)
    {
        if (statAffected == StatAttribute.Speed)
        {
            return 0.5f;
        }
        return base.AlterStat(Holder, statAffected);
    }
    EarnableEV extraEV = new EarnableEV(StatAttribute.Attack, 4);
    public override List<EarnableEV> AdditionalEffortValues(List<EarnableEV> earnedEv)
    {
        earnedEv.Add(extraEV);
        return base.AdditionalEffortValues(earnedEv);
    }
}
