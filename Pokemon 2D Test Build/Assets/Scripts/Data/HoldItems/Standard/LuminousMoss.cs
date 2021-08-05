using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuminousMoss : HoldItemBase
{
    public override HoldItemID Id { get { return HoldItemID.LuminousMoss; } }
    public override HoldItemBase ReturnDerivedClassAsNew() { return new LuminousMoss(); }
    public override StatBoost AlterStatAfterTakingDamageFromCertainType(ElementType attackType)
    {
        if (attackType == ElementType.Water)
        {
            RemoveItem = true;
            return new StatBoost() { stat = StatAttribute.SpecialDefense, boost = 1 };
        }
        return base.AlterStatAfterTakingDamageFromCertainType(attackType);
    }
}
