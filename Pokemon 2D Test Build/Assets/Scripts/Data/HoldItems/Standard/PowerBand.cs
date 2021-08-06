using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBand : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.PowerBand; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new PowerBand(); }
    public override float AlterStat(Pokemon Holder, StatAttribute statAffected)
    {
        if (statAffected == StatAttribute.Speed)
        {
            return 0.5f;
        }
        return base.AlterStat(Holder, statAffected);
    }
    EarnableEV extraEV = new EarnableEV(StatAttribute.SpecialDefense, 4);
    public override List<EarnableEV> AdditionalEffortValues(List<EarnableEV> earnedEv)
    {
        earnedEv.Add(extraEV);
        return base.AdditionalEffortValues(earnedEv);
    }
}
