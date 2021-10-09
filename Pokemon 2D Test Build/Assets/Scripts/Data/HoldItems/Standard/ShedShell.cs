using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShedShell : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.ShedShell; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new ShedShell(); }
    public override bool AlwaysAllowsToSwitchOut()
    {
        return true;
    }
}
