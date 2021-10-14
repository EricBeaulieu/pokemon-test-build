using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaggingTail : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.LaggingTail; } }
    public override int AdjustSpeedPriorityTurn(BattleUnit holder)
    {
        return -1;
    }
}
