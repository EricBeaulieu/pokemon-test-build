using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmuletCoin : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.AmuletCoin; } }
    public override bool DoublesPrizeMoneyRecieved()
    {
        return true;
    }
}
