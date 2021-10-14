using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldItemsNull : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.NA; } }
    public override bool PlayAnimationWhenUsed() { return false; }
}
