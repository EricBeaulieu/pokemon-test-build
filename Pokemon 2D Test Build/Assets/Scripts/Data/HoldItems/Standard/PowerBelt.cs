using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBelt : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.PowerBelt; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new PowerBelt(); }
    public override float AlterStat(Pokemon Holder, StatAttribute statAffected)
    {
        if (statAffected == StatAttribute.Speed)
        {
            return 0.5f;
        }
        return base.AlterStat(Holder, statAffected);
    }
    EarnableEV extraEV = new EarnableEV(StatAttribute.Defense, 4);
    public override List<EarnableEV> AdditionalEffortValues(List<EarnableEV> earnedEv)
    {
        earnedEv.Add(extraEV);
        return base.AdditionalEffortValues(earnedEv);
    }
}