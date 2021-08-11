using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectivePads : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.ProtectivePads; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new ProtectivePads(); }
    public override bool ProtectHolderFromEffectsCausedByMakingDirectContact()
    {
        return true;
    }
}
