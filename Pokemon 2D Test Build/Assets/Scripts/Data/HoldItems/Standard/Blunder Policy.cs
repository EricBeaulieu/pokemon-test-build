using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlunderPolicy : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.BlunderPolicy; } }
    StatBoost statBoost = new StatBoost(StatAttribute.Speed,2);
    public override StatBoost RaisesStatUponMissing(BattleUnit holder)
    {
        holder.removeItem = true;
        return statBoost;
    }
}
