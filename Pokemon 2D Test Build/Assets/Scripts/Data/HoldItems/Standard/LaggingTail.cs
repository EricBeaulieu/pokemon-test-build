using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaggingTail : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.LaggingTail; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new LaggingTail(); }
    public override int AdjustSpeedPriorityTurn()
    {
        return -1;
    }
}
