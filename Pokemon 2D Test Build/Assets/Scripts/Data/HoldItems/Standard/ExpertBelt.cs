using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpertBelt : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.ExpertBelt; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new ExpertBelt(); }
    public override float PowersUpSuperEffectiveAttacks(bool superEffective)
    {
        if (superEffective == false)
        {
            return 1f;
        }
        return 1.2f;
    }
}