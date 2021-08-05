using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaggingTail : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.LaggingTail; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new LaggingTail(); }
    public override bool AlwaysLastInSpeedPriorityTurn()
    {
        return true;
    }
}
