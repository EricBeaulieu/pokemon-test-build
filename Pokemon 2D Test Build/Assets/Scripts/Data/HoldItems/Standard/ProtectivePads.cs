using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectivePads : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.ProtectivePads; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new ProtectivePads(); }
    public override bool ProtectHolderFromEffectsCausedByMakingDirectContact()
    {
        return true;
    }
}
