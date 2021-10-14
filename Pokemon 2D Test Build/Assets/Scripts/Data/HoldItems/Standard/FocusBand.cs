using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusBand : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.FocusBand; } }
    public override bool EndureOHKOAttack(BattleUnit holder)
    {
        if(Random.value <= 0.1f)
        {
            return true;
        }

        return false;
    }
}
