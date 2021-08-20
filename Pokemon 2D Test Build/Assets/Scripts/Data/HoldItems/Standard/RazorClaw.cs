using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RazorClaw : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.RazorClaw; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new RazorClaw(); }
    public override float AlterStat(Pokemon holder, StatAttribute statAffected)
    {
        if (statAffected == StatAttribute.CriticalHitRatio)
        {
            return 1;
        }
        return 0;
    }
}