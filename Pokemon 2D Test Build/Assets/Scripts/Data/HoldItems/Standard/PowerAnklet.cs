using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerAnklet : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.PowerAnklet; } }
    public override float AlterStat(Pokemon Holder, StatAttribute statAffected)
    {
        if (statAffected == StatAttribute.Speed)
        {
            return 0.5f;
        }
        return base.AlterStat(Holder, statAffected);
    }
    EarnableEV extraEV = new EarnableEV(StatAttribute.Speed, 4);
    public override List<EarnableEV> AdditionalEffortValues(List<EarnableEV> earnedEv)
    {
        earnedEv.Add(extraEV);
        return base.AdditionalEffortValues(earnedEv);
    }
}
