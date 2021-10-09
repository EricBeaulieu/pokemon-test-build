using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlunderPolicy : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.BlunderPolicy; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new BlunderPolicy(); }
    StatBoost statBoost = new StatBoost() { stat = StatAttribute.Speed, boost = 2 };
    public override StatBoost RaisesStatUponMissing()
    {
        RemoveItem = true;
        return statBoost;
    }
}
