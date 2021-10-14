using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickClaw : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.QuickClaw; } }
    public override int AdjustSpeedPriorityTurn(BattleUnit holder)
    {
        if(Random.value <= 0.2f)
        {
            return 1;
        }
        return 0;
    }
}
