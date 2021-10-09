using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GripClaw : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.GripClaw; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new GripClaw(); }
    public override bool ExtendsBindToMaxPotential()
    {
        return true;
    }
}
