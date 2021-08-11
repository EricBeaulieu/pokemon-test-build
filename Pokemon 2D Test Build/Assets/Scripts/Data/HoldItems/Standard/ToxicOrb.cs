using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicOrb : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.ToxicOrb; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new ToxicOrb(); }
    public override ConditionID InflictConditionAtTurnEnd()
    {
        return ConditionID.ToxicPoison;
    }
}
