using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachoBrace : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.MachoBrace; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new MachoBrace(); }
    public override float AlterStat(Pokemon Holder, StatAttribute statAffected)
    {
        if(statAffected == StatAttribute.Speed)
        {
            return 0.5f;
        }
        return base.AlterStat(Holder, statAffected);
    }
    List<EarnableEV> updatedEVList = new List<EarnableEV>();
    public override List<EarnableEV> AdditionalEffortValues(List<EarnableEV> earnedEv)
    {
        updatedEVList.Clear();
        foreach (EarnableEV eV in earnedEv)
        {
            updatedEVList.Add(new EarnableEV(eV.statAttribute, eV.statValue * 2));
        }
        return base.AdditionalEffortValues(updatedEVList);
    }
}
