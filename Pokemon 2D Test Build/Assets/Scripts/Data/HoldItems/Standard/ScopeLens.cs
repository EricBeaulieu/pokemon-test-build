using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScopeLens : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.ScopeLens; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new ScopeLens(); }
    public override float AlterStat(Pokemon holder, StatAttribute statAffected)
    {
        if (statAffected == StatAttribute.CriticalHitRatio)
        {
            return 1;
        }
        return 0;
    }
}
