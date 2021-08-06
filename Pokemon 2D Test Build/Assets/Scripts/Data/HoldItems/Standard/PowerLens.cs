using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerLens : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.PowerLens; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new PowerLens(); }
    public override float AlterStat(Pokemon Holder, StatAttribute statAffected)
    {
        if (statAffected == StatAttribute.Speed)
        {
            return 0.5f;
        }
        return base.AlterStat(Holder, statAffected);
    }
    EarnableEV extraEV = new EarnableEV(StatAttribute.SpecialAttack, 4);
    public override List<EarnableEV> AdditionalEffortValues(List<EarnableEV> earnedEv)
    {
        earnedEv.Add(extraEV);
        return base.AdditionalEffortValues(earnedEv);
    }
}
