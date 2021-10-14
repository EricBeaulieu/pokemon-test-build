using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightClay : HoldItemBase
{
    public override HoldItemID HoldItemId { get { return HoldItemID.LightClay; } }
    public override int ShieldDurationBonus()
    {
        return 3;
    }
}
