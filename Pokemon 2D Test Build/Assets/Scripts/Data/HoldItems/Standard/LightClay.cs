using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightClay : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.LightClay; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new LightClay(); }
    public override int ShieldDurationBonus(ShieldType shield)
    {
        if(shield == ShieldType.LightScreen || shield == ShieldType.AuroraVeil)
        {
            return 3;
        }
        return base.ShieldDurationBonus(shield);
    }
}
