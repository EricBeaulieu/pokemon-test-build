using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmuletCoin : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.AmuletCoin; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new AmuletCoin(); }
    public override bool DoublesPrizeMoneyRecieved()
    {
        return true;
    }
}
